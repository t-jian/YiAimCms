

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace YiAim.Cms.Blogs;
public class TagService : CrudAppService<Tag, TagDto, TagDto, long, PagedAndSortedResultRequestDto, TagBaseDto, TagBaseDto>, ITagService
{
    private readonly IRepository<TagMap> _tagMapRepository;
    public TagService(IRepository<Tag, long> repository, IRepository<TagMap> tagMapRepository) : base(repository)
    {
        _tagMapRepository = tagMapRepository;
    }

    public async Task BatchDeleteIds(BatchDeleteIdsInput input)
    {
        var ids = input.Ids.Split(",").Select(n => Convert.ToInt64(n));
        await Repository.DeleteManyAsync(ids);
    }

    public async Task<List<TagAllDto>> GetAll()
    {
        // var res = await Repository.GetListAsync(includeDetails: true);
        var query = from t in await Repository.ToListAsync()
                    join cc in await _tagMapRepository.ToListAsync()
                    on t.Id equals cc.TagId
                    group t by new { t.Id, t.Name, t.Taxis } into newsTable
                    select new TagAllDto
                    {
                        Name = newsTable.Key.Name,
                        Id = newsTable.Key.Id,
                        Count = newsTable.Count(),
                        Taxis = newsTable.Key.Taxis,
                    };
        return query.ToList();
    }
}