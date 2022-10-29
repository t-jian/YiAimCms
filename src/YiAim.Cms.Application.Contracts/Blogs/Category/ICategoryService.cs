using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application;
namespace YiAim.Cms.Blogs;
public interface ICategoryService
{

    Task<List<CategoryDto>> GetAll();
    Task BatchDeleteIds(BatchDeleteIdsInput input);
}
