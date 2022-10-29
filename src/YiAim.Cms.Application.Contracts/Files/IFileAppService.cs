using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace YiAim.Cms.Files;
public interface IFileAppService : IApplicationService
{
    Task<byte[]> GetAsync(string name);

    Task<string> CreateAsync(FileUploadInput input);
}