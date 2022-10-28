using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using YiAim.Cms.EntityFrameworkCore;

namespace YiAim.Cms.Blogs;
public class BlogRepository : EfCoreRepository<CmsDbContext, Blog, int>, IBlogRepository
{
    public BlogRepository(IDbContextProvider<CmsDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
    public async Task BatchInsert(IEnumerable<Blog> blogs)
    {
        var db = await GetDbContextAsync();
        await db.AddRangeAsync(blogs);
        await db.SaveChangesAsync();
    }
}
