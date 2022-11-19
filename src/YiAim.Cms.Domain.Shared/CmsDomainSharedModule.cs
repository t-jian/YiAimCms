using YiAim.Cms.Localization;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;
using YiAim.Cms.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Yitter.IdGenerator;

namespace YiAim.Cms;

[DependsOn(
    typeof(AbpAuditLoggingDomainSharedModule),
    typeof(AbpBackgroundJobsDomainSharedModule),
    typeof(AbpFeatureManagementDomainSharedModule),
    typeof(AbpIdentityDomainSharedModule),
    typeof(AbpOpenIddictDomainSharedModule),
    typeof(AbpPermissionManagementDomainSharedModule),
    typeof(AbpSettingManagementDomainSharedModule),
    typeof(AbpTenantManagementDomainSharedModule)
    )]
public class CmsDomainSharedModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        CmsGlobalFeatureConfigurator.Configure();
        CmsModuleExtensionConfigurator.Configure();
        var configuration = context.Services.GetConfiguration();
        var authorize = new AuthorizeOptions();
        PreConfigure<AuthorizeOptions>(options =>
        {
            var authorizeOption = configuration.GetSection("authorize");
            var githubOption = authorizeOption.GetSection("Github");
            Configure<AuthorizeOptions>(authorizeOption);
            Configure<GithubOptions>(githubOption);
            options.Github = new GithubOptions
            {
                ClientId = githubOption.GetValue<string>(nameof(options.Github.ClientId)),
                ClientSecret = githubOption.GetValue<string>(nameof(options.Github.ClientSecret)),
                RedirectUrl = githubOption.GetValue<string>(nameof(options.Github.RedirectUrl)),
                Scope = githubOption.GetValue<string>(nameof(options.Github.Scope))
            };
            authorize = options;
        });
        PreConfigure<AppOptions>(options =>
        {
            options.Authorize = authorize;
            Configure<AppOptions>(item =>
            {
                item.Authorize = authorize;
            });
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CmsDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<CmsResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/Cms");

            options.DefaultResourceType = typeof(CmsResource);
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("Cms", typeof(CmsResource));
        });
        context.Services.ExecutePreConfiguredActions<AuthorizeOptions>();
    }
}
