using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiAim.Cms.Blogs;

namespace YiAim.Cms.Files
{
    public class FileOptions
    {
        public string BaseRoot { get; set; }
        /// <summary>
        /// 文件上传的根目录
        /// </summary>
        public string FileUploadRootFolder { get; set; } = "Upload";


        /// <summary>
        /// 是否根据文件后缀归类
        /// </summary>
        public bool IsDistinguishType { get; set; } = true;
        public string FilePathFormat { get; set; } = "{FileUploadLocalFolder}/{FileTypeFormat}/{yyyy}/{mm}{dd}/";


        /// <summary>
        /// 允许的文件最大大小单位B
        /// </summary>
        public long MaxFileSize { get; set; } = 5 * 1024 * 1024;//5MB

        /// <summary>
        /// 允许的文件类型
        /// </summary>
        public string[] AllowedUploadFormats { get; set; } = { ".jpg", ".jpeg", ".png", ".gif" };



    }

    public static class FileUtils
    {
        public static FileTypeFormat GetFileTypeFormat(string fileSuffix)
        {
            fileSuffix = fileSuffix.ToLower();
            if (".jpg,.jpeg,.png,.gif,webp".Contains(fileSuffix))
            {
                return FileTypeFormat.Image;
            }
            if (".mp4,.3gp,.avi".Contains(fileSuffix))
            {
                return FileTypeFormat.Video;
            }
            return FileTypeFormat.File;
        }
    }

}
