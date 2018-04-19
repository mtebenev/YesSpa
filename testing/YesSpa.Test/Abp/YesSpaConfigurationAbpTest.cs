using AutoFixture;
using Xunit;
using YesSpa.Abp;

namespace YesSpa.Test.Abp
{
  public class YesSpaConfigurationAbpTest
  {
    [Fact]
    public void Create_Default_Page_Rewrite()
    {
      var fixture = new Fixture();

      var sut = fixture.Create<YesSpaConfigurationAbp>();
      sut.AddSpa("/react/", "YesSpa.Samples.Abp.ClientApp.React.build");

      var pageRewrites = sut.SpaDefaultPageRewrites;

      Assert.Equal(1, pageRewrites.Count);
      Assert.Equal("/react/index.html", pageRewrites[0].DefaultPagePath);
    }
  }
}
