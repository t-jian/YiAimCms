using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Http;
using YiAim.Cms.Files;

namespace YiAim.Cms.Controllers
{
    public class FileController : CmsController
    {
        private readonly IFileAppService _fileAppService;
        public FileController(IFileAppService fileAppService)
        {
            _fileAppService = fileAppService;
        }

        [HttpGet]
        [Route("/api/file/{name}")]
        public async Task<FileResult> GetAsync(string name)
        {
            var bytes = await _fileAppService.GetAsync(name);
            return File(bytes, MimeTypes.GetByExtension(Path.GetExtension(name)));
        }

        [HttpPost]
        [Route("/api/file/upload")]
        public async Task<string> CreateAsync(IFormFile file)
        {
            if (file == null)
                throw new UserFriendlyException("请上传文件");
            var bytes = await file.GetAllBytesAsync();
            var result = await _fileAppService.CreateAsync(new FileUploadInput()
            {
                Bytes = bytes,
                Name = file.FileName
            });
            return await Task.FromResult(result);
        }
    }
}
