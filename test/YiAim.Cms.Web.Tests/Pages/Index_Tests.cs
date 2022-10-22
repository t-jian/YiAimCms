using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace YiAim.Cms.Pages;

public class Index_Tests : CmsWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
