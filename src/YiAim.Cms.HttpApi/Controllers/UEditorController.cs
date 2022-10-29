using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UEditor.Core;
using Volo.Abp.AspNetCore.Mvc;
namespace YiAim.Cms.Controllers;

[Route("/api/[controller]/[action]")]
public class UEditorController : CmsController
{
    private readonly UEditorService _ueditorService;
    public UEditorController(UEditorService ueditorService)
    {
        this._ueditorService = ueditorService;
    }

    [HttpGet, HttpPost]
    public ContentResult Upload()
    {
        var response = _ueditorService.UploadAndGetResponse(HttpContext);
        return Content(response.Result, response.ContentType);
    }
}
