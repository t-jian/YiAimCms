using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace YiAim.Cms.Web;

[Dependency(ReplaceServices = true)]
public class CmsBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Cms";
}
