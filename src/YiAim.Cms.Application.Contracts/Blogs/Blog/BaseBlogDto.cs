using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace YiAim.Cms.Blogs;
public class BaseBlogDto : EntityDto<long>
{
    public string Title { get; set; }
    public int Taxis { get; set; }
    public long? CategoryId { get; set; }
    public long PublishDate { get; set; }
    public string Author { get; set; }
    public bool IsHot { get; set; } 
}
