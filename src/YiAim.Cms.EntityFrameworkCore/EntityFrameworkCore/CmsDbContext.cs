using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using YiAim.Cms.Blogs;
using YiAim.Cms.Users;

namespace YiAim.Cms.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class CmsDbContext :
    AbpDbContext<CmsDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{

    #region Entities from the modules

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    #region blogs

    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Category> Categorys { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<TagMap> TagMaps { get; set; }
    #endregion

    public DbSet<AppUserThirdAuth> AppUserThirdAuths { get; set; }

    public CmsDbContext(DbContextOptions<CmsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();

        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        builder.Entity<Blog>(b =>
        {
            b.ToTable(CmsConsts.CmsDbTablePrefix + "blog", CmsConsts.DbSchema);
            b.Property(n => n.Id).HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.None);
            b.ConfigureByConvention();
        });
       
        builder.Entity<Category>(b =>
        {
            b.ToTable(CmsConsts.CmsDbTablePrefix + "category", CmsConsts.DbSchema);
            b.Property(n => n.Id).HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.None);
            b.ConfigureByConvention();
        });
        builder.Entity<Tag>(b =>
        {
            b.ToTable(CmsConsts.CmsDbTablePrefix + "tag", CmsConsts.DbSchema);
            b.Property(n => n.Id).HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.None);
            b.ConfigureByConvention();
        });

        builder.Entity<TagMap>(b =>
        {
            b.ToTable(CmsConsts.CmsDbTablePrefix + "tag_map", CmsConsts.DbSchema);
            b.HasKey(e => new { e.BlogId, e.TagId });
        });
        builder.Entity<AppUserThirdAuth>(b =>
        {
            b.ToTable(CmsConsts.CmsDbTablePrefix + "user_third_auth", CmsConsts.DbSchema);
            b.ConfigureByConvention();
        });
    }
}
