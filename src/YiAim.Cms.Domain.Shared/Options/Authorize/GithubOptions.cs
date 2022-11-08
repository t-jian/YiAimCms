using System;
using System.Collections.Generic;
using System.Text;

namespace YiAim.Cms.Options;
public class GithubOptions : AuthOptions, IAuthOptions
{
    public string AuthorizeUrl => "https://github.com/login/oauth/authorize";

    public string AccessTokenUrl => "https://github.com/login/oauth/access_token";

    public string UserInfoUrl => "https://api.github.com/user";
}
