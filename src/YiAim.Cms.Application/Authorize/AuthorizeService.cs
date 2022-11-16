using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using YiAim.Cms.Users;

namespace YiAim.Cms.Authorize
{
    public class AuthorizeService : ApplicationService, IAuthorizeService
    {
        private readonly GithubService _githubService;
        private readonly ILogger _logger;
        private readonly IRepository<AppUserThirdAuth, int> _appUserThirdAuth;
        private readonly IdentityUserManager _IdentityUserManager;
        protected AbpSignInManager _signInManager { get; }
        private IHttpClientFactory _httpClient;
        public AuthorizeService(IHttpClientFactory httpClient, AbpSignInManager SignInManager, IdentityUserManager identityUserManager, GithubService githubService, IRepository<AppUserThirdAuth, int> appUserThirdAuth, ILogger<AuthorizeService> logger)
        {
            _githubService = githubService;
            _logger = logger;
            _appUserThirdAuth = appUserThirdAuth;
            _IdentityUserManager = identityUserManager;
            _signInManager = SignInManager;
            _httpClient = httpClient;
        }
        /// <summary>
        /// 获取授权用户信息
        /// 流程
        /// 1、拿到code、state  换取access_token, 
        /// 2、根据access_token 换取用户相关信息
        /// </summary>
        /// <param name="type">授权类型 github\gitee</param>
        /// <param name="code">授权码</param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("oauth/{type}/GetAuthUserInfo")]
        public async Task<AuthUserBaseInfo> GetAuthUserInfo(string type, string code, string state)
        {
            if (!StateManager.IsAny(state))
                throw new UserFriendlyException("请求过期");
            StateManager.Remove(state);
            try
            {
                var result = type switch
                {
                    "github" => await _githubService.GetUserByOAuthAsync(type, code, state),
                    _ => throw new UserFriendlyException($"Not implemented:{type}")
                };
                return result.FieldMap();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
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

        /// <summary>
        /// 获取token 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        [HttpGet]
        [Route("oauth/{type}/token")]
        public async Task<string> GetTokenAsync(string type, string code, string state)
        {
            if (!StateManager.IsAny(state))
                throw new UserFriendlyException("请求失败");
            StateManager.Remove(state);
            try
            {
                var result = type switch
                {
                    "github" => await _githubService.GetAccessTokenAsync(code, state),
                    _ => throw new UserFriendlyException($"Not implemented:{type}")
                };
                return result.AccessToken;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        [HttpGet]
        [Route("oauth/{type}/thirdAuthLogin")]
        public async Task<string> ThirdAuthLogin(IdentityType type, string code, string state)
        {
            var tokenResult = type switch
            {
                IdentityType.github => await _githubService.GetAccessTokenAsync(code, state),
                _ => throw new UserFriendlyException($"Not implemented:{type}")
            };
            if (tokenResult != null && !string.IsNullOrWhiteSpace(tokenResult.AccessToken))
            {
                //通过accesstoken 获取用户信息
                //获取授权用户信息后，查看是否绑定AppUserThirdAuth
                //绑定有则通过调用/connect/token,生成token返回
                var userInfo = type switch
                {
                    IdentityType.github => await _githubService.GetUserInfoAsync(tokenResult.AccessToken),
                    _ => throw new UserFriendlyException($"Not implemented:{type}")
                };
                if (await _appUserThirdAuth.AnyAsync(n => n.Identifier == userInfo.Id && n.IdentityType == type))
                {
                    var appUserThirdAuth = await _appUserThirdAuth.SingleAsync(n => n.Identifier == userInfo.Id && n.IdentityType == type);
                    var identityUser = await _IdentityUserManager.GetByIdAsync(appUserThirdAuth.UserId);
                    var str = await _IdentityUserManager.CreateSecurityTokenAsync(identityUser);
                    // await _signInManager.SignInAsync(identityUser, false);
                    var token = await _IdentityUserManager.GenerateUserTokenAsync(identityUser, "PasswordlessLoginProvider", "passwordless-auth");
                    var client = _httpClient.CreateClient();
                    var dic = new Dictionary<string, object>
                {
                    {"client_id","Cms_App"},
                    {"client_secret",""},
                    {"scope","Cms"},
                    {"grant_type","password"},
                    {"username",identityUser.UserName},
                    {"password",identityUser.PasswordHash},
                };
                    var dicStr = dic.Select(m => m.Key + "=" + m.Value).DefaultIfEmpty().Aggregate((m, n) => m + "&" + n);
                    HttpContent httpContent = new StringContent(dicStr);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    var oauthRep = await client.PostAsync("https://localhost:44377/connect/token", httpContent);
                    var oauthStr = await oauthRep.Content.ReadAsStringAsync();

                    Console.WriteLine(oauthStr);

                    return "11";
                }
                else
                {
                    throw new UserFriendlyException($"用户未绑定:{type}");
                }
            }
            else
            {
                throw new UserFriendlyException($"获取{type}授权失败，请重试！");
            }
        }
    }
}
