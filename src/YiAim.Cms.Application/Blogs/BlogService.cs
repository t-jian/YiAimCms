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

namespace YiAim.Cms.Blogs;
public class BlogService : CrudAppService<Blog, BlogDetailDto, PageBlogDto, int, PagingInput, CreateBlogInput, UpdateBlogInput>, IBlogService
{
    private readonly IRepository<Category, int> _categoryRepository;
    private readonly IRepository<Tag, int> _tagRepository;
    private readonly IRepository<TagMap> _tagMapRepository;
    public BlogService(
        IRepository<Category, int> categoryRepository,
        IRepository<Tag, int> tagRepository,
        IRepository<TagMap> tagMapRepository,
        IRepository<Blog, int> repository) : base(repository)
    {
        _tagRepository = tagRepository;
        _tagMapRepository = tagMapRepository;
        _categoryRepository = categoryRepository;
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
        Blog blog = await Repository.InsertAsync(ObjectMapper.Map<CreateBlogInput, Blog>(input),true);
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
}
