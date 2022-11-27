using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace YiAim.Cms.Blogs;
public class BlogDetailDto : BaseBlogDto, IEntityDto<long>
{
    public string Content { get; set; }
    public long Id { get ; set ; }
}

