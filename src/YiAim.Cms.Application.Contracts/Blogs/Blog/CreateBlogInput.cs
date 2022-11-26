using System;
using System.Collections.Generic;
using System.Text;

namespace YiAim.Cms.Blogs;
public class CreateBlogInput:BaseBlogDto
{

    /// <summary>
    /// 详情
    /// </summary>
    public string Content { get; set; }
    public string Extend { get; set; }
    /// <summary>
    /// 标签
    /// </summary>
    public string Tags { get; set; }
}
