using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using YiAim.Cms.Blogs;

namespace YiAim.Cms.Controllers;

public class HelloAbpController : CmsController
{
    private readonly IBlogService _blogService;
    public HelloAbpController(IBlogService blogService)
    {
        _blogService = blogService;
    }
   
}
