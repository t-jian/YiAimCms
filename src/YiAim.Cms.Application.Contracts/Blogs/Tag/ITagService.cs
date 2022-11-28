using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace YiAim.Cms.Blogs;
public interface ITagService:ICrudAppService<TagDto, TagDto, long, PagedAndSortedResultRequestDto, TagBaseDto, TagBaseDto>
{
    Task<List<TagAllDto>> GetAll();
    Task BatchDeleteIds(BatchDeleteIdsInput input);
}
