using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace YiAim.Cms.Users;
public class AppUserThirdAuth : AuditedEntity<int>
{
    [NotNull]
    /// <summary>
    /// 关联用户id
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 第三方用户唯一标识 如openid
    /// </summary>
    [NotNull]
    [MaxLength(50)]
    public string Identifier { get; set; }
    /// <summary>
    /// 第三方平台登录类型(手机号/邮箱) 或第三方应用名称 (github/微信/微博等)
    /// </summary>
    [NotNull]
    [MaxLength(50)]
    public IdentityType IdentityType { get; set; }

    [MaxLength(50)]
    public string AccessToken { get; set; }
    /// <summary>
    /// 头像
    /// </summary>
    [MaxLength(255)]
    public string Avatar { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [MaxLength(150)]
    public string NickName { get; set; }

}
