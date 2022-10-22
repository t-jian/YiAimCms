using YiAim.Cms.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace YiAim.Cms;

[DependsOn(
    typeof(CmsEntityFrameworkCoreTestModule)
    )]
public class CmsDomainTestModule : AbpModule
{

}
