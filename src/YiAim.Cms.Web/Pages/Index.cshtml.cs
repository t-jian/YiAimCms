using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using YiAim.Cms.Blogs;

namespace YiAim.Cms.Web.Pages;

public class IndexModel : CmsPageModel
{
    private readonly IBlogService _blogService;
    private readonly IAnthologyService _AnthologyService;
    public IndexModel(IBlogService blogService, IAnthologyService anthologyService)
    {
        _blogService = blogService;
        _AnthologyService = anthologyService;
    }
    public PagedResultDto<BlogClientDto> Blogs { get; set; }
    public List<BlogClientDto> Blog48List { get; set; }
    public List<BlogClientDto> HotRandomBlogs { get; set; }
    public List<BlogClientDto> RandomBlogs { get; set; }
    public List<PageAnthologyClientDto> Anthologys { get; set; }
    public PageAnthologyClientDto LastAnthology { get; set; }
    public async Task OnGet()
    {
        Blogs = await _blogService.GetPageBlogClient(null,1, 20);
        Blog48List = await _blogService.GetHotBlogsClient(9, true);
        RandomBlogs = await _blogService.GetRandomBlogsClient(9);
        HotRandomBlogs = await _blogService.GetRandomBlogsClient(6);
        Anthologys = await _AnthologyService.GetRandomAnthologyClient();
        LastAnthology = await _AnthologyService.GetLastAnthologyClient();
        //await _AnthologyService.GetLastAnthologyClient();
    }
}
