using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YiAim.Cms.Data;
using Volo.Abp.DependencyInjection;

namespace YiAim.Cms.EntityFrameworkCore;

public class EntityFrameworkCoreCmsDbSchemaMigrator
    : ICmsDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreCmsDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {

        await _serviceProvider
            .GetRequiredService<CmsDbContext>()
            .Database
            .MigrateAsync();
    }
}
