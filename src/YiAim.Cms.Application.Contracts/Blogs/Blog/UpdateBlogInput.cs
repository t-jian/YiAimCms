using System;
using System.Collections.Generic;
using System.Text;

namespace YiAim.Cms.Blogs;
public class UpdateBlogInput:BaseBlogDto
{
    /// <summary>
    /// 详情
    /// </summary>
    public string Content { get; set; }
    public string Extend { get; set; }
    public string Tags { get; set; }
}
