using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace YiAim.Cms.Authorize
{
    public class GithubAuthDto : AuthUserBaseInfo, IAuthUserInfoFieldMap
    {
        [JsonProperty("login")]
        public override string UserName { get; set; }

        public AuthUserBaseInfo FieldMap()
        {
            this.NickName = this.UserName;
            return this;
        }
    }
}
