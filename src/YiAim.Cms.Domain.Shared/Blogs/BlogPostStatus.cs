using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace YiAim.Cms.Blogs;
public enum BlogPostStatus
{
    /// <summary>
    /// 草稿
    /// </summary>
    [Description("草稿")]
    Draft,
    [Description("已发布")]
    Published,
    [Description("待审核")]
    WaitingForReview
}
