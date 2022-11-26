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
using System.Linq.Dynamic.Core;
using Volo.Abp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace YiAim.Cms.Blogs;


public class AnthologyService : CrudAppService<Anthology, AnthologyDto, PageAnthologyDto, long, PagedAndSortedResultRequestDto, CreateAnthologyInput, UpdateAnthologyInput>, IAnthologyService
{
    private readonly IRepository<AnthologyInBlog> _anthologyInBlogRepository;
    public AnthologyService(IRepository<Anthology, long> repository, IRepository<AnthologyInBlog> anthologyInBlogRepository) : base(repository)
    {
        _anthologyInBlogRepository = anthologyInBlogRepository;
    }

    public async Task<List<PageAnthologyClientDto>> GetHotAnthologyClient(int limit = 10)
    {
        var queryable = await Repository.GetQueryableAsync();
        var queryResult = await AsyncExecuter.ToListAsync(queryable.Where(n => n.Status == BlogPostStatus.Published && n.IsHot)
            .OrderByDescending(n => n.CreationTime).Take(limit));
        var result = queryResult.Select(n => { return ObjectMapper.Map<Anthology, PageAnthologyClientDto>(n); }).ToList();
        result.ForEach(async n => { n.Count = await _anthologyInBlogRepository.CountAsync(n => n.AnthologyId == n.AnthologyId); });
        return result;
    }

    public async Task<PageAnthologyClientDto> GetLastAnthologyClient()
    {
        var queryable = await Repository.GetQueryableAsync();
        var queryResult = await AsyncExecuter.FirstOrDefaultAsync(queryable.Where(n => n.Status == BlogPostStatus.Published)
           .OrderByDescending(n => n.CreationTime));
        if (queryResult == null)
        {
            throw new EntityNotFoundException(typeof(Anthology));
        }
        var dto = ObjectMapper.Map<Anthology, PageAnthologyClientDto>(queryResult);
        dto.Count = await _anthologyInBlogRepository.CountAsync(n => n.AnthologyId == n.AnthologyId);
        return dto;
    }

    public async Task<List<PageAnthologyClientDto>> GetRandomAnthologyClient(int limit = 10)
    {
        var queryable = await Repository.GetQueryableAsync();
        var queryResult = await AsyncExecuter.ToListAsync(queryable.Where(n => n.Status == BlogPostStatus.Published)
            .OrderBy(n => Guid.NewGuid()).Take(limit));
        var result = queryResult.Select(n => { return ObjectMapper.Map<Anthology, PageAnthologyClientDto>(n); }).ToList();
        result.ForEach(async n => { n.Count = await _anthologyInBlogRepository.CountAsync(n => n.AnthologyId == n.AnthologyId); });
        return result;
    }
}
