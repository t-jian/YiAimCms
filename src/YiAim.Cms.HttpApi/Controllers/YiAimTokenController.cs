using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.OpenIddict.Controllers;
using YiAim.Cms.Authorize;
using YiAim.Cms.Users;

namespace YiAim.Cms.Controllers;


[Route("/ym/connect/token")]
public class YiAimTokenController : TokenController
{
    private readonly IAuthorizeService _authorizeService;
    private readonly IRepository<AppUserThirdAuth, Guid> _appUserThirdAuth;
    public YiAimTokenController(IAuthorizeService authorizeService, IRepository<AppUserThirdAuth, Guid> appUserThirdAuth)
    {
        _authorizeService = authorizeService;
        _appUserThirdAuth = appUserThirdAuth;
    }
    public override async Task<IActionResult> HandleAsync()
    {
        var request = await GetOpenIddictServerRequestAsync(HttpContext);

        if (request.GrantType.Equals(GlobalConstant.OpeniddictGrantType_ThirdAuth))
        {
            var form = HttpContext.Request.Form;
            string type = form["type"];
            string code = request.Code;
            string state = request.State;
            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(state) || string.IsNullOrWhiteSpace(type))
            {
                return Forbid(GetAuthenticationProperties($"参数异常"), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            var userInfo = await _authorizeService.GetAuthUserInfo(type, code, state);
            if (userInfo is null)
            {
                return Forbid(GetAuthenticationProperties($"获取{type}授权信息异常，请重试！"), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            IdentityType identityType = (IdentityType)Enum.Parse(typeof(IdentityType), type);
            if (!await _appUserThirdAuth.AnyAsync(n => n.Identifier == userInfo.Id && n.IdentityType == identityType))
            {
                return Forbid(GetAuthenticationProperties($"未绑定{type}"), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            var appUserThirdAut = await _appUserThirdAuth.FindAsync(n => n.Identifier == userInfo.Id && n.IdentityType == identityType);
            IdentityUser user = await UserManager.GetByIdAsync(appUserThirdAut.UserId);
            if (user is null)
            {
                return Forbid(GetAuthenticationProperties($"绑定用户不存在"), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            //设置用户登录
            await SignInManager.SignInAsync(user, isPersistent: false);
            return await SetSuccessResultAsync(request, user);
        }
        else
        {
            return await base.HandleAsync();
        }
    }
    private AuthenticationProperties GetAuthenticationProperties(string msg)
    {
        return new AuthenticationProperties(new Dictionary<string, string>
        {
            [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidRequest,
            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = msg
        });
    }
}

