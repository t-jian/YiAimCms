using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace YiAim.Cms.Blogs;

public interface IAnthologyService : ICrudAppService<AnthologyDto, PageAnthologyDto, long, PagedAndSortedResultRequestDto, CreateAnthologyInput, UpdateAnthologyInput>
{
    Task<List<PageAnthologyClientDto>> GetRandomAnthologyClient(int limit = 10);
    Task<PageAnthologyClientDto> GetLastAnthologyClient();
    Task<List<PageAnthologyClientDto>> GetHotAnthologyClient(int limit = 10);

}
