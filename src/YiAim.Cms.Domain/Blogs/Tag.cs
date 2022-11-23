using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Yitter.IdGenerator;

namespace YiAim.Cms.Blogs;
public class Tag : AuditedEntity<long>, ITaxis
{
    public Tag()
    {
        this.Id = YitIdHelper.NextId();
        TagMaps =new HashSet<TagMap>();
    }
    [NotNull]
    [MaxLength(150)]
    public string Name { get; set; }
    public int Taxis { get; set; } = 0;
    public ICollection<TagMap> TagMaps { get; set; }
    public void SetId(long id)
    {
        this.Id = id;
    }
}
