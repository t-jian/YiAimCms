using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YiAim.Cms.EntityFrameworkCore;
using YiAim.Cms.Localization;
using YiAim.Cms.MultiTenancy;
using YiAim.Cms.Web.Menus;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity.Web;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement.Web;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement.Web;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using System;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using System.Net.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using OpenIddict.Server;
using System.Threading.Tasks;

namespace YiAim.Cms.Web;

[DependsOn(
    typeof(CmsHttpApiModule),
    typeof(CmsApplicationModule),
    typeof(CmsEntityFrameworkCoreModule),
    typeof(AbpAutofacModule),
    typeof(AbpIdentityWebModule),
    typeof(AbpSettingManagementWebModule),
    typeof(AbpAccountWebOpenIddictModule),
    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
    typeof(AbpTenantManagementWebModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule)
    )]
public class CmsWebModule : AbpModule
{
    private const string DefaultCorsPolicyName = "Default";
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(CmsResource),
                typeof(CmsDomainModule).Assembly,
                typeof(CmsDomainSharedModule).Assembly,
                typeof(CmsApplicationModule).Assembly,
                typeof(CmsApplicationContractsModule).Assembly,
                typeof(CmsWebModule).Assembly
            );
        });

        PreConfigure<OpenIddictBuilder>(builder =>
        {
            builder.AddValidation(options =>
            {
                options.AddAudiences("Cms");
                options.UseLocalServer();
                options.UseAspNetCore();

            });

        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();
        ConfigureAuthentication(context, configuration);
        ConfigureUrls(configuration);
        ConfigureBundles();
        ConfigureAutoMapper();
        ConfigureVirtualFileSystem(hostingEnvironment);
        ConfigureFile(hostingEnvironment);
        ConfigureLocalizationServices();
        ConfigureNavigationServices();
        ConfigureAutoApiControllers();
        //配置跨域
        ConfigureCors(context, configuration);
        ConfigureSwaggerServices(context, configuration);
        context.Services.AddHttpClient();

        context.Services.AddOpenIddict()
            .AddServer(option =>
            {
                option.AllowCustomFlow(GlobalConstant.OpeniddictGrantType_ThirdAuth);
                option.SetTokenEndpointUris(new[] { "/ym/connect/token" });
            });
    }
    private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["AuthServer:Authority"];
                options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                options.Audience = "Cms";
                options.SaveToken = true;
                options.BackchannelHttpHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                           HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            });


    }
    private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
    {

        context.Services.AddAbpSwaggerGenWithOAuth(
              configuration["AuthServer:Authority"],//授权地址，要跟OpenIddict里面的地址一致
              new Dictionary<string, string>() { { "Cms", "Cms Swagger API" } },//字典第一个是授权的作用域，第二是描述可以随意填写
              options =>
              {
                  options.SwaggerDoc("v1", new OpenApiInfo { Title = "忆目内容管理系统 API", Version = "v1" });
                  options.DocInclusionPredicate((docName, description) => true);
                  options.CustomSchemaIds(type => type.FullName);
              }
         );
    }
    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddCors(options =>
        {
            options.AddPolicy(DefaultCorsPolicyName, builder =>
            {
                builder
                    .WithOrigins(
                        configuration["App:CorsOrigins"]
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(o => o.RemovePostFix("/"))
                            .ToArray()
                    )
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }
    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
        });
    }

    private void ConfigureBundles()
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(
                LeptonXLiteThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddFiles("/global-styles.css");
                }
            );
        });
    }
    private void ConfigureAutoMapper()
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<CmsWebModule>();
        });
    }
    private void ConfigureFile(IWebHostEnvironment hostingEnvironment)
    {
        Configure<Files.FileOptions>(options =>
        {
            options.BaseRoot = hostingEnvironment.ContentRootPath;
            options.FileUploadRootFolder = "staticfiles";
        });
    }
    private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
    {
        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<CmsDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}YiAim.Cms.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<CmsDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}YiAim.Cms.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<CmsApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}YiAim.Cms.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<CmsApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}YiAim.Cms.Application"));
                options.FileSets.ReplaceEmbeddedByPhysical<CmsWebModule>(hostingEnvironment.ContentRootPath);
            });
        }
    }

    private void ConfigureLocalizationServices()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Languages.Add(new LanguageInfo("en", "en", "English"));
            options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
        });
    }

    private void ConfigureNavigationServices()
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new CmsMenuContributor());
        });
    }

    private void ConfigureAutoApiControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(CmsApplicationModule).Assembly);
        });
    }


    private void ConfigureStaticFiles(IApplicationBuilder app)
    {
        app.UseStaticFiles();
        string staticFileRoot = Path.Combine(Directory.GetCurrentDirectory(), "staticfiles");
        if (!System.IO.Directory.Exists(staticFileRoot))
            System.IO.Directory.CreateDirectory(staticFileRoot);
        app.UseStaticFiles(new StaticFileOptions()
        {
            FileProvider = new PhysicalFileProvider(staticFileRoot),
            RequestPath = new PathString("/staticfiles"),
            //OnPrepareResponse = ctx =>
            //{
            //    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=36000");
            //}
        });
    }
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
        }

        app.UseCorrelationId();
        ConfigureStaticFiles(app);

        app.UseRouting();
        app.UseCors(DefaultCorsPolicyName);
        app.UseAuthentication();

        app.UseAbpOpenIddictValidation();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }

        app.UseUnitOfWork();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "默认接口");
            //var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            options.OAuthClientId("Cms_Swagger");
            //options.OAuthClientSecret("1q2w3e*");
            // options.ConfigObject.AdditionalItems = "ss";
        });

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}
