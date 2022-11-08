using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp.DependencyInjection;
using YiAim.Cms.Options;
using YiAim.Cms.Extensions;
using System.Net.Http.Headers;

namespace YiAim.Cms.Authorize
{
    public abstract class ThirdOAuthServiceBase<TOptions, TAccessToke, TUserInfo> : IOAuthService<TAccessToke, TUserInfo>, ITransientDependency where TOptions : class where TAccessToke : class where TUserInfo : class
    {
        protected readonly object ServiceProviderLock = new object();
        //  public IDistributedCache Cache { get; set; }
        public IServiceProvider ServiceProvider { get; set; }

        public IOptions<TOptions> Options { get; set; }


        private IHttpClientFactory _httpClient;


        protected IHttpClientFactory HttpClient => LazyGetRequiredService(ref _httpClient);

        protected TService LazyGetRequiredService<TService>(ref TService reference)
        {
            return LazyGetRequiredService(typeof(TService), ref reference);
        }

        protected TRef LazyGetRequiredService<TRef>(Type serviceType, ref TRef reference)
        {
            if (reference == null)
            {
                lock (ServiceProviderLock)
                {
                    if (reference == null)
                    {
                        reference = (TRef)ServiceProvider.GetRequiredService(serviceType);
                    }
                }
            }

            return reference;
        }
        /// <summary>
        /// 获取授权地址
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual async Task<string> GetAuthorizeUrl(string state)
        {
            var param = BuildAuthorizeUrlParams(state);
            IAuthOptions authOptions = Options.Value as IAuthOptions;
            var url = $"{authOptions.AuthorizeUrl}?{param.ToQueryString()}";
            return await Task.FromResult(url);
        }
        /// <summary>
        /// 根据授权信息获取用户信息
        /// </summary>
        /// <param name="type">授权类型</param>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual async Task<TUserInfo> GetUserByOAuthAsync(string type, string code, string state)
        {
            var accessToken = await GetAccessTokenAsync(code, state);
            var curAccessToken = accessToken as AccessTokenBase;
            return await GetUserInfoAsync(curAccessToken.AccessToken);
        }
        /// <summary>
        /// 获取授权的access token
        /// </summary>
        /// <param name="code">code</param>
        /// <param name="state">state</param>
        /// <returns></returns>
        public virtual async Task<TAccessToke> GetAccessTokenAsync(string code, string state)
        {
            var param = BuildAccessTokenParams(code, state);
            var content = new StringContent(param.ToQueryString());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            using var client = HttpClient.CreateClient();
            IAuthOptions authOptions = Options.Value as IAuthOptions;
            var httpResponse = await client.PostAsync(authOptions.AccessTokenUrl, content);
            var response = await httpResponse.Content.ReadAsStringAsync();
            try
            {
                return JsonConvert.DeserializeObject<TAccessToke>(response);
            }
            catch
            {
                var qscoll = HttpUtility.ParseQueryString(response);
                var aa = new AccessTokenBase
                {
                    AccessToken = qscoll["access_token"],
                    Scope = qscoll["scope"],
                    TokenType = qscoll["token_type"],
                    ExpiresIn = 84000
                };
                return aa as TAccessToke;
            }
        }
        /// <summary>
        ///根据access token 获取授权的用户信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public virtual async Task<TUserInfo> GetUserInfoAsync(string accessToken)
        {
            using var client = HttpClient.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", $"token {accessToken}");
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36 Edg/87.0.664.66");
            IAuthOptions authOptions = Options.Value as IAuthOptions;
            var response = await client.GetStringAsync(authOptions.UserInfoUrl);
            return JsonConvert.DeserializeObject<TUserInfo>(response);
        }
        public virtual Dictionary<string, string> BuildAuthorizeUrlParams(string state) => throw new NotImplementedException();
        public virtual Dictionary<string, string> BuildAccessTokenParams(string code, string state) => throw new NotImplementedException();

    }
}
