using YiAim.Cms.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace YiAim.Cms.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class CmsController : AbpControllerBase
{
    protected CmsController()
    {
        LocalizationResource = typeof(CmsResource);
    }
}
