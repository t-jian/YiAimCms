using System.Threading.Tasks;

namespace YiAim.Cms.Data;

public interface ICmsDbSchemaMigrator
{
    Task MigrateAsync();
}
