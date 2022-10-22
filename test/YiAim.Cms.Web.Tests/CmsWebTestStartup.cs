using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp;

namespace YiAim.Cms;

public class CmsWebTestStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication<CmsWebTestModule>();
    }

    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
        app.InitializeApplication();
    }
}
