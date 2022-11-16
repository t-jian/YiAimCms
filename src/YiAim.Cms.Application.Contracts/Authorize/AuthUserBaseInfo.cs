using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace YiAim.Cms.Authorize
{
    public interface IAccessToken
    {
        string AccessToken { get; set; }
    }

    public interface IAuthUserInfoFieldMap
    {
        /// <summary>
        /// 字段映射成 AuthUserBaseInfo 对象里面的属性
        /// </summary>
        /// <returns></returns>
        AuthUserBaseInfo FieldMap();
    }
    /// <summary>
    /// 定义授权返回的基本数据
    /// 最终所有第三方授权信息都必须映射成功该实体类
    /// </summary>
    public class AuthUserBaseInfo: IAccessToken
    {
        /// <summary>
        /// 唯一标识，不能为空
        /// </summary>
        [JsonProperty("id")]
        public virtual string Id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [JsonProperty("username")]
        public virtual string UserName { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [JsonProperty("nickname")]
        public virtual string NickName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        [JsonProperty("avatar_url")]
        public virtual string Avatar { get; set; }

        public virtual string AccessToken { get; set; }

    }

  

}
