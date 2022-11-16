using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YiAim.Cms.Users;

namespace YiAim.Cms.Authorize
{
    public interface IAuthorizeService
    {
        /// <summary>
        /// 获取登录地址(GitHub)
        /// </summary>
        /// <returns></returns>
        Task<string> GetAuthorizeUrlAsync(string type);
        /// <summary>
        /// 获取授权用户信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        Task<AuthUserBaseInfo> GetAuthUserInfo(string type, string code, string state);

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        Task<string> GetTokenAsync(string type, string code, string state);

        /// <summary>
        /// 第三方授权登录
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns>授权成功直接返回token</returns>
        Task<string> ThirdAuthLogin(IdentityType type , string code, string state);
    }
}
