using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace YiAim.Cms.Blogs;

public class AnthologyBaseDto
{
    public int Taxis { get; set; } = 0;
    public string Title { get; set; }
    public string Digest { get; set; }
    public string Thumb { get; set; }
    public bool IsHot { get; set; }
    public DateTime? CreationTime { get; set; }
}
public class AnthologyDto : AnthologyBaseDto, IEntityDto<long>
{
    [NotNull]
    public BlogPostStatus Status { get; set; }
    public long Id { get; set; }
}

public class PageAnthologyDto : AnthologyDto
{

}

public class CreateAnthologyInput : AnthologyBaseDto
{

}
public class UpdateAnthologyInput : AnthologyBaseDto, IEntityDto<long>
{
    [NotNull]
    public BlogPostStatus Status { get; set; }
    public long Id { get; set; }
}

public class PageAnthologyClientDto : PageAnthologyDto
{
    public int Count { get; set; }
}