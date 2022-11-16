using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiAim.Cms.Options;

namespace YiAim.Cms.Authorize
{
    public class GithubService : ThirdOAuthServiceBase<GithubOptions, AccessTokenBase, GithubAuthDto>
    {
        public override Dictionary<string, string> BuildAuthorizeUrlParams(string state)
        {
            return new Dictionary<string, string>
            {
                ["client_id"] = Options.Value.ClientId,
                ["redirect_uri"] = Options.Value.RedirectUrl,
                ["scope"] = Options.Value.Scope,
                ["state"] = state
            };
        }

        public override Dictionary<string, string> BuildAccessTokenParams(string code, string state)
        {
            return new Dictionary<string, string>()
            {
                ["client_id"] = Options.Value.ClientId,
                ["client_secret"] = Options.Value.ClientSecret,
                ["redirect_uri"] = Options.Value.RedirectUrl,
                ["code"] = code,
                ["state"] = state
            };
        }

    }
}
