using AutoFixture;
using Microsoft.AspNetCore.Http;
using Xunit;
using YesSpa.Common.Configuration;
using YesSpa.Test.Testing;

namespace YesSpa.Test.Common.Configuration
{
  public class DefaultPageRewriteTest
  {
    /// <summary>
    /// When configured with non empty paths
    /// </summary>
    [Fact]
    public void Should_Work_With_Non_Root_Paths()
    {
      var fixture = new Fixture()
        .Customize(new DefaultPageRewriteCustomization {RootUrlPath = "/react/"})
        .Customize(new DefaultPageWriterCustomization());

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
      Assert.True(sut.IsMatching(new PathString("/another/path")));
    }

    /// <summary>
    /// Support for deep links: should serve subpaths
    /// </summary>
    [Fact]
    public void Should_Work_With_Nested_Paths()
    {
      var fixture = new Fixture()
        .Customize(new DefaultPageRewriteCustomization {RootUrlPath = "/angular"});

      var sut = fixture.Create<DefaultPageRewrite>();

      Assert.True(sut.IsMatching(new PathString("/angular/")));
      Assert.True(sut.IsMatching(new PathString("/angular/module1")));
      Assert.True(sut.IsMatching(new PathString("/angular/module1/")));
      Assert.True(sut.IsMatching(new PathString("/angular/module2")));
      Assert.True(sut.IsMatching(new PathString("/angular/module2/")));
    }
  }
}
