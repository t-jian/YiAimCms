using YiAim.Cms.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace YiAim.Cms.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(CmsEntityFrameworkCoreModule),
    typeof(CmsApplicationContractsModule)
    )]
public class CmsDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
    }
}
