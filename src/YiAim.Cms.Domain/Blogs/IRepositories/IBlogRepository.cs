using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace YiAim.Cms.Blogs;
public interface IBlogRepository : IBasicRepository<Blog, int>
{
    Task<List<Blog>> GetListAsync(
        string filter = null,
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
        );

    Task<long> GetCountAsync(
        string filter = null,
        CancellationToken cancellationToken = default
        );


    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
