using System;
using System.Collections.Generic;
using System.Text;

namespace YiAim.Cms.Blogs;
public class UpdateBlogInput
{
    /// <summary>
    /// 分类
    /// </summary>
    public int? CategoryId { get; set; }

    public string Title { get; set; }
    /// <summary>
    /// 作者
    /// </summary>
    public string Author { get; set; }
    /// <summary>
    /// 来源
    /// </summary>
    public string Source { get; set; }
    /// <summary>
    /// 来源Url
    /// </summary>
    public string LinkUrl { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    public int Taxis { get; set; } = 0;

    /// <summary>
    /// 缩略图
    /// </summary>
    public string ThumbImg { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public BlogPostStatus Status { get; set; } = BlogPostStatus.WaitingForReview;
    /// <summary>
    /// 描述
    /// </summary>
    public string Digest { get; set; }

    /// <summary>
    /// 详情
    /// </summary>
    public string Content { get; set; }
    /// <summary>
    /// 发布日期
    /// </summary>
    public long PublishDate { get; set; }
    public string Extend { get; set; }
    public string Tags { get; set; }
}
