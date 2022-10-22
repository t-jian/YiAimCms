using Volo.Abp.Modularity;

namespace YiAim.Cms;

[DependsOn(
    typeof(CmsApplicationModule),
    typeof(CmsDomainTestModule)
    )]
public class CmsApplicationTestModule : AbpModule
{

}
