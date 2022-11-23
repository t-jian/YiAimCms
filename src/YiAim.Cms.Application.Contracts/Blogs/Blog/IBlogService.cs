using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace YiAim.Cms.Blogs;

public interface IBlogService :ICrudAppService<BlogDetailDto, PageBlogDto, long, PagedAndSortedResultRequestDto, CreateBlogInput, UpdateBlogInput>
{

    Task UpdateTaxis(UpdateBlogCategoryInput input);
    Task BatchDeleteIds(BatchDeleteIdsInput input);
}

