using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
namespace YiAim.Cms.Blogs;
public class BaseCategoryDto
{
    public virtual string Title { get; set; }
    public virtual int Taxis { get; set; } = 1;
}
public class CategoryDto : EntityDto<int>
{
    public string Title { get; set; }
    public int Taxis { get; set; }
}

public class CreateCategoryInput : BaseCategoryDto{}

public class EditCategoryInput : BaseCategoryDto { }