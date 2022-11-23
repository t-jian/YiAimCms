using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Guids;

namespace YiAim.Cms.Blogs;
public class Material :  AuditedEntity<Guid>
{
   
    /// <summary>
    /// 文件名称（上传时候的文件名）
    /// </summary>
    [MaxLength(100)]
    public string FileOriginName { get; set; }

    /// <summary>
    /// 文件存储位置
    /// </summary>
    public FileLocationType FileLocation { get; set; } = FileLocationType.Location;
    /// <summary>
    /// 文件大小kb
    /// </summary>
    [MaxLength(10)]
    public string FileSizeKb { get; set; }

    /// <summary>
    /// 文件后缀
    /// </summary>
    [MaxLength(50)]
    public string FileSuffix { get; set; }
    /// <summary>
    /// 存储路径
    /// </summary>
    [MaxLength(255)]
    public string FilePath { get; set; }
    /// <summary>
    /// 引用数量
    /// </summary>
    public int QuoteTotal { get; set; } = 0;

    /// <summary>
    /// 存储第三方平台（文件唯一标识id）
    /// </summary>
    [MaxLength(100)]
    public string FileThirdKey { get; set; }=string.Empty;

    public string FileHash { get; set; }
    public void SetId(Guid id)
    {
        this.Id = id;
    }
}
