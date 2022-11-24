using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using YiAim.Cms.Blogs;

namespace YiAim.Cms.Web.Pages;

public class IndexModel : CmsPageModel
{
    private readonly IBlogService _BlogService;
    public IndexModel(IBlogService BlogService)
    {
        _BlogService = BlogService;
    }
    public PagedResultDto<PageBlogDto> Blogs { get; set; }
    public async Task OnGet()
    {
        Blogs = await _BlogService.GetListAsync(new PagedAndSortedResultRequestDto() { SkipCount = 0, MaxResultCount = 10 });
    }
}
