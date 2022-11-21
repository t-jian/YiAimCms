using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace YiAim.Cms.Blogs;

/// <summary>
/// 标签与文章关联表
/// </summary>
public class TagMap : Entity
{
    [NotNull]
    public long TagId { get; set; }
    [NotNull]
    public long BlogId { get; set; }

    public override object[] GetKeys()
    {
        return new object[] { TagId, TagId };
    }
}
