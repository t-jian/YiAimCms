using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace YiAim.Cms.Blogs;
public interface IBlogRepository : IRepository<Blog, int>
{
    Task BatchInsert(IEnumerable<Blog> blogs);
}
