using System;
using System.Collections.Generic;
using System.Text;
using YiAim.Cms.Localization;
using Volo.Abp.Application.Services;

namespace YiAim.Cms;

/* Inherit your application services from this class.
 */
public abstract class CmsAppService : ApplicationService
{
    protected CmsAppService()
    {
        LocalizationResource = typeof(CmsResource);
    }
}
