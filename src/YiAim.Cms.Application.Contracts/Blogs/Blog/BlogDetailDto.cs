using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace YiAim.Cms.Blogs;
public class BlogDetailDto : BaseBlogDto
{
    public string Source { get; set; }
    /// <summary>
    /// 外部链接地址
    /// </summary>
    public string LinkUrl { get; set; }

    /// <summary>
    /// 文章缩略图
    /// </summary>
    public string ThumbImg { get; set; }


    public BlogPostStatus Status { get; set; }

    public string Digest { get; set; }
 
    public string Content { get; set; }
}
