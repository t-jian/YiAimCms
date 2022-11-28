using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
namespace YiAim.Cms.Blogs;

public class TagBaseDto
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    public int Taxis { get; set; }
}
public class TagDto : TagBaseDto, IEntityDto<long>
{
    public long Id { get; set; }
}

public class TagAllDto : TagDto
{
    public int Count { get; set; }
}