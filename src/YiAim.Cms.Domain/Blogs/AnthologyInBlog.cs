using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace YiAim.Cms.Blogs;

/// <summary>
///文集里面的文章
/// </summary>
public class AnthologyInBlog : Entity
{
    [NotNull]
    public long AnthologyId { get; set; }
    [NotNull]
    public long BlogId { get; set; }

    public override object[] GetKeys()
    {
        return new object[] { AnthologyId, BlogId };
    }

}
