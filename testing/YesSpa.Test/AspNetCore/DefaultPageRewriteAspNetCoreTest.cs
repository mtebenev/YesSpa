using AutoFixture;
using Microsoft.AspNetCore.Http;
using Xunit;
using YesSpa.AspNetCore;
using YesSpa.Test.Testing;

namespace YesSpa.Test.AspNetCore
{
  public class DefaultPageRewriteAspNetCoreTest
  {
    /// <summary>
    /// When configured with non empty paths
    /// </summary>
    [Fact]
    public void Should_Work_With_Non_Root_Paths()
    {
      var fixture = new Fixture()
        .Customize(new DefaultPageRewriteAspNetCoreCustomization {RootUrlPath = "/react/", DefaultPagePath = "/embedded-path"});

      var sut = fixture.Create<DefaultPageRewriteAspNetCore>();

      var matchResult = sut.MatchRequest(new PathString("/react"));
      Assert.True(matchResult.matches);
      Assert.Equal("/embedded-path/index.html", matchResult.newPath);

      matchResult = sut.MatchRequest(new PathString("/react/"));
      Assert.True(matchResult.matches);
      Assert.Equal("/embedded-path/index.html", matchResult.newPath);

      matchResult = sut.MatchRequest(new PathString("/another/path"));
      Assert.False(matchResult.matches);

      matchResult = sut.MatchRequest(new PathString("/"));
      Assert.False(matchResult.matches);
    }

    /// <summary>
    /// When configured to serve site root
    /// </summary>
    [Fact]
    public void Should_Work_With_Root_Paths()
    {
      var fixture = new Fixture()
        .Customize(new DefaultPageRewriteAspNetCoreCustomization { RootUrlPath = "/", DefaultPagePath = "/embedded-path" });

      var sut = fixture.Create<DefaultPageRewriteAspNetCore>();

      var matchResult = sut.MatchRequest(new PathString("/"));
      Assert.True(matchResult.matches);
      Assert.Equal("/embedded-path/index.html", matchResult.newPath);

      matchResult = sut.MatchRequest(new PathString("/another/path"));
      Assert.True(matchResult.matches);
      Assert.Equal("/embedded-path/index.html", matchResult.newPath);
    }

    /// <summary>
    /// Support for deep links: should serve subpaths
    /// </summary>
    [Fact]
    public void Should_Work_With_Nested_Paths()
    {
      var fixture = new Fixture()
        .Customize(new DefaultPageRewriteAspNetCoreCustomization { RootUrlPath = "/angular", DefaultPagePath = "/embedded-path" });

      var sut = fixture.Create<DefaultPageRewriteAspNetCore>();

      var matchResult = sut.MatchRequest(new PathString("/angular/"));
      Assert.True(matchResult.matches);
      Assert.Equal("/embedded-path/index.html", matchResult.newPath);

      matchResult = sut.MatchRequest(new PathString("/angular/module1"));
      Assert.True(matchResult.matches);
      Assert.Equal("/embedded-path/index.html", matchResult.newPath);

      matchResult = sut.MatchRequest(new PathString("/angular/module1/"));
      Assert.True(matchResult.matches);
      Assert.Equal("/embedded-path/index.html", matchResult.newPath);

      matchResult = sut.MatchRequest(new PathString("/angular/module2"));
      Assert.True(matchResult.matches);
      Assert.Equal("/embedded-path/index.html", matchResult.newPath);

      matchResult = sut.MatchRequest(new PathString("/angular/module2/"));
      Assert.True(matchResult.matches);
      Assert.Equal("/embedded-path/index.html", matchResult.newPath);
    }

    [Fact]
    public void Should_Return_Index_If_No_Embedded_Resource()
    {
      var fixture = new Fixture()
        .Customize(new DefaultPageRewriteAspNetCoreCustomization
        {
          RootUrlPath = "/angular",
          DefaultPagePath = "/embedded-path",
          EmbeddedFilePath = "/embedded-resource"
        });

      var sut = fixture.Create<DefaultPageRewriteAspNetCore>();

      var matchResult = sut.MatchRequest(new PathString("/angular/some-page"));
      Assert.True(matchResult.matches);
      Assert.Equal("/embedded-path/index.html", matchResult.newPath);
    }

    [Fact]
    public void Should_Return_Embedded_Resource()
    {
      var fixture = new Fixture()
        .Customize(new DefaultPageRewriteAspNetCoreCustomization
        {
          RootUrlPath = "/angular",
          DefaultPagePath = "/embedded-path",
          EmbeddedFilePath = "/embedded-path/embedded-resource"
        });

      var sut = fixture.Create<DefaultPageRewriteAspNetCore>();

      var matchResult = sut.MatchRequest(new PathString("/angular/embedded-resource"));
      Assert.True(matchResult.matches);
      Assert.Equal("/embedded-path/embedded-resource", matchResult.newPath);
    }
  }
}
