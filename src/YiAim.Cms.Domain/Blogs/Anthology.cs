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

/// <summary>
///文集
/// </summary>
public class Anthology : AuditedEntity<long>, ITaxis
{
    public Anthology()
    {
        Id = YitIdHelper.NextId();
        AnthologyInBlogs = new HashSet<AnthologyInBlog>();
    }
    public int Taxis { get; set; }
    [MaxLength(254)]
    [NotNull]
    public string Title { get; set; }
    [MaxLength(254)]
    public string Digest { get; set; }
    public string Thumb { get; set; }
    public bool IsHot { get; set; }
    /// <summary>
    /// 审核状态
    /// </summary>
    [NotNull]
    public BlogPostStatus Status { get; set; }

    public ICollection<AnthologyInBlog> AnthologyInBlogs { get; set; }

}
