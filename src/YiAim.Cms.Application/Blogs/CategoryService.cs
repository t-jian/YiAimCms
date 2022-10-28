using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System;
using Volo.Abp;

namespace YiAim.Cms.Blogs;
public class CategoryService : CrudAppService<Category, CategoryDto, int, PagingInput, CreateCategoryInput, EditCategoryInput>, ICategoryService
{
    public CategoryService(IRepository<Category, int> repository) : base(repository)
    {
    }
    public override async Task<CategoryDto> CreateAsync(CreateCategoryInput input)
    {
        if (await Repository.AnyAsync(n => n.Title.Equals(input.Title)))
        {
            throw new UserFriendlyException("分类名称已经存在");
        }
        return await base.CreateAsync(input);
    }

    [HttpGet("/api/app/Category/GetAll")]
    
    public async Task<List<CategoryDto>> GetAll()
    {
        var items = await Repository.GetListAsync();
        return ObjectMapper.Map<List<Category>, List<CategoryDto>>(items);
    }
}
