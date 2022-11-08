using System;
using System.Collections.Generic;
using System.Text;

namespace YiAim.Cms.Options;

public abstract class AuthOptions
{

    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string RedirectUrl { get; set; }
    public string Scope { get; set; }
}

public interface IAuthOptions
{
    /// <summary>
    /// 授权地址
    /// </summary>
    string AuthorizeUrl { get; }
    /// <summary>
    /// accesstoken url
    /// </summary>
    string AccessTokenUrl { get; }
    /// <summary>
    /// 获取用户信息地址
    /// </summary>
    string UserInfoUrl { get; }
}

