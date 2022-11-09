using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace YiAim.Cms.Authorize
{
    public class AuthorizeService : ApplicationService, IAuthorizeService
    {
        private readonly GithubService _githubService;
        private readonly ILogger _logger;
        public AuthorizeService(GithubService githubService, ILogger<AuthorizeService> logger)
        {
            _githubService = githubService;
            _logger = logger;
        }
        /// <summary>
        /// 获取授权成功后token 
        /// 流程
        /// 1、拿到code、state  换取access_token, 
        /// 2、根据access_token 换取用户相关信息
        /// 3、根据用户唯一信息换取用户表的相关信息
        /// 4、根据用户信息生成token
        /// </summary>
        /// <param name="type">授权类型 github\gitee</param>
        /// <param name="code">授权码</param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("oauth/{type}/token")]
        public async Task<dynamic> GenerateTokenAsync(string type, string code, string state)
        {
            if (!StateManager.IsAny(state))
                throw new Exception("请求失败");
            StateManager.Remove(state);
            var result = type switch
            {
                "github" => await _githubService.GetUserByOAuthAsync(type, code, state),
                _ => throw new NotImplementedException($"Not implemented:{type}")
            };
            return result;
        }

        /// <summary>
        /// 获取授权链接
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("oauth/{type}")]
        public async Task<string> GetAuthorizeUrlAsync(string type)
        {
            var state = StateManager.Instance.Get();
            string url = type switch
            {
                "github" => await _githubService.GetAuthorizeUrl(state),
                _ => throw new NotImplementedException($"Not implemented:{type}")
            };
            return url;
        }
    }
}
