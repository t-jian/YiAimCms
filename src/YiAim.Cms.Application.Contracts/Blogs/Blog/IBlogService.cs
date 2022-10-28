using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace YiAim.Cms.Blogs;

public interface IBlogService : IApplicationService
{
    #region 用于后台的接口
    Task Add(AddBlogInput input);
    Task<PagedList<PageBlogDto>> Page(PagingInput requestDto);
    #endregion
}

