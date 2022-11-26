using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using YiAim.Cms.Blogs;

namespace YiAim.Cms.Web.Pages;

public class IndexModel : CmsPageModel
{
    private readonly IBlogService _BlogService;
    private readonly IAnthologyService _AnthologyService;
    public IndexModel(IBlogService BlogService, IAnthologyService anthologyService)
    {
        _BlogService = BlogService;
        _AnthologyService = anthologyService;
    }
    public PagedResultDto<PageBlogDto> Blogs { get; set; }
    public List<PageAnthologyClientDto> Anthologys { get; set; }
    public PageAnthologyClientDto LastAnthology { get; set; }
    public async Task OnGet()
    {
        Blogs = await _BlogService.GetListAsync(new PagedAndSortedResultRequestDto() { SkipCount = 0, MaxResultCount = 10 });
        Anthologys = await _AnthologyService.GetRandomAnthologyClient();
        LastAnthology = await _AnthologyService.GetLastAnthologyClient();
    }
}
