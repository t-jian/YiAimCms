using YiAim.Cms.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using YiAim.Cms.Blogs;
using System.Threading.Tasks;

namespace YiAim.Cms.Controllers;

/* Inherit your controllers from this class.
 */
public abstract  class CmsController : AbpControllerBase
{
    protected CmsController()
    {
        LocalizationResource = typeof(CmsResource);
    }
 
}
