using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace YiAim.Cms.Blogs;

public enum FileTypeFormat
{
    [Description("图片")]
    Image,
    [Description("文件")]
    File,
    [Description("视频")]
    Video,
    [Description("音频")]
    Audio
}