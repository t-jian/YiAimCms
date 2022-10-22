using YiAim.Cms.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace YiAim.Cms.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class CmsPageModel : AbpPageModel
{
    protected CmsPageModel()
    {
        LocalizationResourceType = typeof(CmsResource);
    }
}
