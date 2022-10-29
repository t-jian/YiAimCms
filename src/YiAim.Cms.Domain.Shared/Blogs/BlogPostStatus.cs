using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace YiAim.Cms.Blogs;
public enum BlogPostStatus
{
    [Description("已发布")]
    Published = 1,
    [Description("草稿")]
    Draft=2,
    [Description("待审核")]
    WaitingForReview=3
}
