using YiAim.Cms.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace YiAim.Cms.Permissions;

public class CmsPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(CmsPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(CmsPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CmsResource>(name);
    }
}
