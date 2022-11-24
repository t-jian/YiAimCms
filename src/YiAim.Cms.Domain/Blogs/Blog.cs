using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Yitter.IdGenerator;

namespace YiAim.Cms.Blogs;
public class Blog : AuditedEntity<long>, ITaxis
{

    public Blog()
    {
        this.Id = YitIdHelper.NextId();
        TagMaps = new HashSet<TagMap>();
        AnthologyInBlogs = new HashSet<AnthologyInBlog>();
    }

    [NotNull]
    [MaxLength(254)]
    public string Title { get; set; }
    [MaxLength(100)]
    public string Author { get; set; }
    /// <summary>
    /// 是否为热点或推荐
    /// </summary>
    public bool IsHot { get; set; } = false;
    /// <summary>
    /// 来源
    /// </summary>
    [MaxLength(150)]
    public string Source { get; set; }

    /// <summary>
    /// 外部链接地址
    /// </summary>
    public string LinkUrl { get; set; }

    /// <summary>
    /// 文章缩略图
    /// </summary>
    public string ThumbImg { get; set; }

    /// <summary>
    /// 审核状态
    /// </summary>
    [NotNull]
    public BlogPostStatus Status { get; set; }

    /// <summary>
    /// 文章摘要
    /// </summary>
    [MaxLength(254)]
    public string Digest { get; set; }
    /// <summary>
    /// 文章内容 已进行编码 ，js 端使用 encodeURIComponent解码
    /// 后端采用System.Web.HttpUtility.UrlDecode(s)解码 / UrlEncoder.Default.Encode(s) 编码
    /// </summary>
    public string Content { get; set; }
    /// <summary>
    /// 发布时间
    /// </summary>
    public long PublishDate { get; set; }
    public int Taxis { get; set; } = 0;

    public long? CategoryId { get; set; }
    public virtual ICollection<TagMap> TagMaps { get; set; }
    public virtual ICollection<AnthologyInBlog> AnthologyInBlogs { get; set; }

    public void SetId(long id)
    {
        this.Id = id;
    }
}
