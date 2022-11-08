using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System;
using Volo.Abp;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace YiAim.Cms.Blogs;

[Authorize]
public class CategoryService : CrudAppService<Category, CategoryDto, int, PagingInput, CreateCategoryInput, EditCategoryInput>, ICategoryService
{
    public CategoryService(IRepository<Category, int> repository) : base(repository)
    {
    }

    [HttpPost("/api/app/category/BatchDeleteIds")]
    public async Task BatchDeleteIds(BatchDeleteIdsInput input)
    {
        int[] intids = input.Ids.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
        await Repository.DeleteManyAsync(intids);
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
