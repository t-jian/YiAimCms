using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace YiAim.Cms.Users;
public class AppUserThirdAuth : AuditedEntity<int>
{
    /// <summary>
    /// 关联用户id
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 第三方用户唯一标识 如openid
    /// </summary>
    public string Identifier { get; set; }
    /// <summary>
    /// 第三方平台登录类型(手机号/邮箱) 或第三方应用名称 (github/微信/微博等)
    /// </summary>
    public IdentityType IdentityType { get; set; }

    public string AccessToken { get; set; }
    
}
