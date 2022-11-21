using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Validation;
using YiAim.Cms.Blogs;
/// <summary>
/// 文件管理不需要自动生成API
/// </summary>
namespace YiAim.Cms.Files;
[RemoteService(IsMetadataEnabled = false)]
public class FileAppService : ApplicationService, IFileAppService
{
    private readonly FileOptions _fileOptions;

    public FileAppService(IOptions<FileOptions> fileOptions)
    {
        _fileOptions = fileOptions.Value;
    }

    public Task<byte[]> GetAsync(string name)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        var filePath = Path.Combine(_fileOptions.BaseRoot, _fileOptions.FileUploadRootFolder, name);

        if (File.Exists(filePath))
        {
            return Task.FromResult(File.ReadAllBytes(filePath));
        }
        return Task.FromResult(new byte[0]);
    }

    public Task<string> CreateAsync(FileUploadInput input)
    {
        if (input.Bytes.IsNullOrEmpty())
        {
            throw new AbpValidationException("Bytes can not be null or empty!",
                new List<ValidationResult>
                {
                    new ValidationResult("Bytes can not be null or empty!", new[] {"Bytes"})
                });
        }

        if (input.Bytes.Length > _fileOptions.MaxFileSize)
        {
            throw new UserFriendlyException($"File exceeds the maximum upload size ({_fileOptions.MaxFileSize / 1024 / 1024} MB)!");
        }

        if (!_fileOptions.AllowedUploadFormats.Contains(Path.GetExtension(input.Name)))
        {
            throw new UserFriendlyException("Not a valid file format!");
        }
        string path = _fileOptions.FilePathFormat.Replace("{FileUploadLocalFolder}", _fileOptions.FileUploadRootFolder);
        path = path.Replace("{FileTypeFormat}", _fileOptions.IsDistinguishType ? FileUtils.GetFileTypeFormat(Path.GetExtension(input.Name)).ToString() : FileTypeFormat.File.ToString());
        path = path.Replace("{yyyy}", DateTime.Now.ToString("yyyy"))
                .Replace("{mm}", DateTime.Now.ToString("MM"))
                .Replace("{dd}", DateTime.Now.ToString("dd"));
        var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(input.Name);
        var filePath = Path.Combine(_fileOptions.BaseRoot, path, fileName);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        File.WriteAllBytes(filePath, input.Bytes);
        return Task.FromResult("/" + path + fileName);
    }
}

