using AutoFixture;
using Microsoft.AspNetCore.Http;
using Xunit;
using YesSpa.Common.Configuration;

namespace YesSpa.Test.Common.Configuration
{
  public class DefaultPageRewriteCustomization : ICustomization
  {
    public string RootUrlPath { get; set; }
    public void Customize(IFixture fixture)
    {
      fixture
        .Customize<DefaultPageRewrite>(c => c.FromFactory(() => new DefaultPageRewrite(RootUrlPath, fixture.Create<string>())));
    }
  }

  public class DefaultPageRewriteTest
  {
    /// <summary>
    /// When configured with non empty paths
    /// </summary>
    [Fact]
    public void Should_Work_With_Non_Root_Paths()
    {
      var fixture = new Fixture()
        .Customize(new DefaultPageRewriteCustomization {RootUrlPath = "/react/"});

      var sut = fixture.Create<DefaultPageRewrite>();

      Assert.True(sut.IsMatching(new PathString("/react")));
      Assert.True(sut.IsMatching(new PathString("/react/")));

      Assert.False(sut.IsMatching(new PathString("/another/path")));
      Assert.False(sut.IsMatching(new PathString("/")));
    }

    /// <summary>
    /// When configured to serve site root
    /// </summary>
    [Fact]
    public void Should_Work_With_Root_Paths()
    {
      var fixture = new Fixture()
        .Customize(new DefaultPageRewriteCustomization {RootUrlPath = "/"});

      var sut = fixture.Create<DefaultPageRewrite>();

      Assert.True(sut.IsMatching(new PathString("/")));
      Assert.False(sut.IsMatching(new PathString("/another/path")));
    }


  }
}
