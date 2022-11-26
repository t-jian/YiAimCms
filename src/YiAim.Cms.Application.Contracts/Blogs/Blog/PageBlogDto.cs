using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace YiAim.Cms.Blogs;
public class PageBlogDto : BaseBlogDto, IEntityDto<long>
{
    public long Id { get; set; }
}
