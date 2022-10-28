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

namespace YiAim.Cms.Blogs;
public class BlogService : ApplicationService, IBlogService
{
    private readonly IBlogRepository _blogRepository;
    public BlogService(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    public Task Add(AddBlogInput input)
    {
        return Task.FromResult("");
    }

    public async Task<PagedList<PageBlogDto>> Page(PagingInput requestDto)
    {
        PagedList<PageBlogDto> pagedResult = new();
        var items = await _blogRepository.GetPagedListAsync((requestDto.Page-1)*requestDto.Limit, requestDto.Limit, "");
        pagedResult.Items= ObjectMapper.Map<List<Blog>, List<PageBlogDto>>(items);
        return pagedResult;
    }
}
