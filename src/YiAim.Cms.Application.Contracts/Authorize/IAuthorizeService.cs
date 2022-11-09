using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
        /// 获取AccessToken
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        Task<dynamic> GenerateTokenAsync(string type, string code, string state);

    }
}
