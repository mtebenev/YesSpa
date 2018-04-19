using AutoFixture;
using Xunit;
using YesSpa.AspNetCore;

namespace YesSpa.Test.AspNetCore
{
  public class YesSpaConfigurationAspNetCoreTest
  {
    [Fact]
    public void Create_Default_Page_Rewrite()
    {
      var fixture = new Fixture();

      var sut = fixture.Create<YesSpaConfigurationAspNetCore>();
      sut.AddSpa("/react/", "/.Modules/AspNetCore.ClientApp.React/build");

      var pageRewrites = sut.SpaDefaultPageRewrites;

      Assert.Equal(1, pageRewrites.Count);
      Assert.Equal("/.Modules/AspNetCore.ClientApp.React/build/index.html", pageRewrites[0].DefaultPagePath);
    }
  }
}
