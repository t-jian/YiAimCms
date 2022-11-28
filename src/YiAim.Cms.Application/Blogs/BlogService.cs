using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System;
using Volo.Abp;
using System.Text.Encodings.Web;
using Volo.Abp.Uow;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.ObjectMapping;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography;
using System.Collections;

namespace YiAim.Cms.Blogs;


public class BlogService : CrudAppService<Blog, BlogDetailDto, PageBlogDto, long, PagedAndSortedResultRequestDto, CreateBlogInput, UpdateBlogInput>, IBlogService
{
    private readonly IRepository<Category, long> _categoryRepository;
    private readonly IRepository<Tag, long> _tagRepository;
    private readonly IRepository<TagMap> _tagMapRepository;
    public BlogService(
        IRepository<Category, long> categoryRepository,
        IRepository<Tag, long> tagRepository,
        IRepository<TagMap> tagMapRepository,
        IRepository<Blog, long> repository) : base(repository)
    {
        _tagRepository = tagRepository;
        _tagMapRepository = tagMapRepository;
        _categoryRepository = categoryRepository;
    }
    public override Task<PagedResultDto<PageBlogDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        return base.GetListAsync(input);
    }
    [HttpPost("/api/app/blog/UpdateTaxis")]
    public async Task UpdateTaxis(UpdateBlogCategoryInput input)
    {
        if (!(await _categoryRepository.AnyAsync(n => n.Id == input.ColumnId)))
        {
            throw new UserFriendlyException("分类不存在");
        }
        var blog = await Repository.FirstOrDefaultAsync(n => n.Id == input.NewsId);
        if (blog is null)
        {
            throw new UserFriendlyException("文章不存在");
        }
        blog.CategoryId = input.ColumnId;
        await Repository.UpdateAsync(blog);
    }

    [UnitOfWork]
    public async override Task<BlogDetailDto> CreateAsync(CreateBlogInput input)
    {
        List<Tag> tags = new();
        if (!string.IsNullOrWhiteSpace(input.Tags))
        {
            foreach (string tagStr in input.Tags.Split(','))
            {
                Tag tag = await _tagRepository.FirstOrDefaultAsync(n => n.Name.Equals(tagStr));
                if (tag is null)
                {
                    tag = new Tag
                    {
                        Name = tagStr,
                        Taxis = 0,
                    };
                    await _tagRepository.InsertAsync(tag, true);
                }
                tags.Add(tag);
            }
        }
        if (string.IsNullOrWhiteSpace(input.Digest))
        {
            string desc = "";
            //RichTextHtmlHelper.ReplaceAllTag(input.Content);
            if (desc.Length > 200)
                desc = desc.Substring(0, desc.Length - 1);
            input.Digest = desc;
        }
        //编码存入数据库
        input.Content = UrlEncoder.Default.Encode(input.Content);
        Blog blog = await Repository.InsertAsync(ObjectMapper.Map<CreateBlogInput, Blog>(input), true);
        if (tags.Count > 0)
        {
            List<TagMap> articleTags = new();
            tags.ForEach(tag =>
            {
                articleTags.Add(new TagMap { BlogId = blog.Id, TagId = tag.Id });
            });
            await _tagMapRepository.InsertManyAsync(articleTags);
        }
        return ObjectMapper.Map<Blog, BlogDetailDto>(blog);
    }
    [UnitOfWork]
    public override async Task<BlogDetailDto> UpdateAsync(long id, UpdateBlogInput input)
    {
        var blog = await Repository.FirstOrDefaultAsync(n => n.Id == id);
        if (blog is null)
            throw new UserFriendlyException("文章不存在");
        //先解除所有标签关联

        var tm = await _tagMapRepository.GetListAsync(n => n.BlogId == blog.Id);
        if (tm.Count() > 0)
            await _tagMapRepository.DeleteManyAsync(tm.ToList());

        List<Tag> tags = new();
        if (!string.IsNullOrWhiteSpace(input.Tags))
        {
            foreach (string tagStr in input.Tags.Split(','))
            {
                Tag tag = await _tagRepository.FirstOrDefaultAsync(n => n.Name.Equals(tagStr));
                if (tag is null)
                {
                    tag = new Tag
                    {
                        Name = tagStr,
                        Taxis = 0,
                    };
                    await _tagRepository.InsertAsync(tag, true);
                }
                tags.Add(tag);
            }
        }
        if (string.IsNullOrWhiteSpace(input.Digest))
        {
            string desc = "";
            //RichTextHtmlHelper.ReplaceAllTag(input.Content);
            if (desc.Length > 200)
                desc = desc.Substring(0, desc.Length - 1);
            input.Digest = desc;
        }
        //编码存入数据库
        input.Content = UrlEncoder.Default.Encode(input.Content);
        blog.Title = input.Title;
        blog.ThumbImg = input.ThumbImg;
        blog.Author = input.Author;
        blog.Status = input.Status;
        blog.Content = input.Content;
        blog.PublishDate = input.PublishDate;
        blog.Taxis = input.Taxis;
        blog.Digest = input.Digest;
        blog.Source = input.Source;
        await Repository.UpdateAsync(blog);
        if (tags.Count > 0)
        {
            List<TagMap> articleTags = new();
            tags.ForEach(tag =>
            {
                articleTags.Add(new TagMap { BlogId = blog.Id, TagId = tag.Id });
            });
            await _tagMapRepository.InsertManyAsync(articleTags);
        }
        return ObjectMapper.Map<Blog, BlogDetailDto>(blog);
    }

    [UnitOfWork]
    [HttpPost("/api/app/blog/BatchDeleteIds")]
    public async Task BatchDeleteIds(BatchDeleteIdsInput input)
    {
        //删除要删除关联的标签、图片等资源
        foreach (string id in input.Ids.Split(','))
        {
            await DeleteById(Convert.ToInt32(id));
        }
    }

    [UnitOfWork]
    private async Task DeleteById(int id)
    {
        Blog blog = await Repository.FirstOrDefaultAsync(b => b.Id == id);
        if (blog is not null)
        {
            //这里可以使用其他方法直接删除，不用查询
            var tagmaps = await _tagMapRepository.GetListAsync();
            if (tagmaps.Count > 0)
                await _tagMapRepository.DeleteManyAsync(tagmaps);
            await Repository.DeleteAsync(blog);
        }
    }

    public async Task<List<BlogClientDto>> GetRandomBlogsClient(int limit = 10)
    {
        var query = await GetBlogs(c => c.Status == BlogPostStatus.Published);
        return query.OrderBy(n => Guid.NewGuid()).Take(limit).ToList();
    }

    public async Task<List<BlogClientDto>> GetHotBlogsClient(int limit = 10, bool isRandom = false)
    {
        var query = await GetBlogs(c => c.Status == BlogPostStatus.Published && c.IsHot);
        if (isRandom)
            query = query.OrderBy(n => Guid.NewGuid());
        else
            query = query.OrderByDescending(n => n.PublishDate);
        return query.Take(limit).ToList();
    }
    private async Task<IEnumerable<BlogClientDto>> GetBlogs(Func<Blog, bool> func)
    {
        return from c in await Repository.GetListAsync()
               join a in await _categoryRepository.GetListAsync() on c.CategoryId equals a.Id into tempTab
               where func.Invoke(c)
               orderby c.PublishDate descending
               from temp in tempTab.DefaultIfEmpty()
               group temp by new
               {
                   c.Id,
                   c.IsHot,
                   c.CategoryId,
                   c.Digest,
                   c.Status,
                   c.Source,
                   c.PublishDate,
                   c.Author,
                   c.Title,
                   c.ThumbImg,
                   c.Taxis,
                   c.LinkUrl,
                   cateTitle = temp.Title
               } into item
               select new BlogClientDto
               {
                   Id = item.Key.Id,
                   Digest = item.Key.Digest,
                   PublishDate = item.Key.PublishDate,
                   Author = item.Key.Author,
                   Category = new BaseCategoryDto { Taxis = 0, Title = item.Key.cateTitle },
                   CategoryId = item.Key.CategoryId,
                   IsHot = item.Key.IsHot,
                   Taxis = item.Key.Taxis,
                   LinkUrl = item.Key.LinkUrl,
                   Source = item.Key.Source,
                   Status = item.Key.Status,
                   ThumbImg = item.Key.ThumbImg,
                   Title = item.Key.Title,
               };
    }
    public async Task<PagedResultDto<BlogClientDto>> GetPageBlogClient(long? cid, int page, int limit)
    {

        var responses = await GetBlogs(c => c.Status == BlogPostStatus.Published && cid != null && cid > 0 ? c.CategoryId == cid : true);
        int total = responses.Count();
        return new PagedResultDto<BlogClientDto>() { Items = responses.Skip((page - 1) * limit).Take(limit).ToList(), TotalCount = total };
    }
}
