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
        .Customize(new DefaultPageRewriteCustomization {RootUrlPath = "/react/", DefaultPagePath = "/embedded-path"})
        .Customize(new SpaPageWriterCustomization());

      var sut = fixture.Create<DefaultPageRewrite>();
      string newPathRequest;

      Assert.True(sut.MatchRequest(new PathString("/react"), out newPathRequest));
      Assert.Equal("/embedded-path/index.html", newPathRequest);

      Assert.True(sut.MatchRequest(new PathString("/react/"), out newPathRequest));
      Assert.Equal("/embedded-path/index.html", newPathRequest);

      Assert.False(sut.MatchRequest(new PathString("/another/path"), out newPathRequest));
      Assert.False(sut.MatchRequest(new PathString("/"), out newPathRequest));
    }

    /// <summary>
    /// When configured to serve site root
    /// </summary>
    [Fact]
    public void Should_Work_With_Root_Paths()
    {
      var fixture = new Fixture()
        .Customize(new DefaultPageRewriteCustomization {RootUrlPath = "/", DefaultPagePath = "/embedded-path" });

      var sut = fixture.Create<DefaultPageRewrite>();
      string newPathRequest;

      Assert.True(sut.MatchRequest(new PathString("/"), out newPathRequest));
      Assert.Equal("/embedded-path/index.html", newPathRequest);

      Assert.True(sut.MatchRequest(new PathString("/another/path"), out newPathRequest));
      Assert.Equal("/embedded-path/another/path", newPathRequest);
    }

    /// <summary>
    /// Support for deep links: should serve subpaths
    /// </summary>
    [Fact]
    public void Should_Work_With_Nested_Paths()
    {
      var fixture = new Fixture()
        .Customize(new DefaultPageRewriteCustomization {RootUrlPath = "/angular", DefaultPagePath = "/embedded-path" });

      var sut = fixture.Create<DefaultPageRewrite>();
      string newPathRequest;

      Assert.True(sut.MatchRequest(new PathString("/angular/"), out newPathRequest));
      Assert.Equal("/embedded-path/index.html", newPathRequest);

      Assert.True(sut.MatchRequest(new PathString("/angular/module1"), out newPathRequest));
      Assert.Equal("/embedded-path/module1", newPathRequest);

      Assert.True(sut.MatchRequest(new PathString("/angular/module1/"), out newPathRequest));
      Assert.Equal("/embedded-path/module1", newPathRequest);

      Assert.True(sut.MatchRequest(new PathString("/angular/module2"), out newPathRequest));
      Assert.Equal("/embedded-path/module2", newPathRequest);

      Assert.True(sut.MatchRequest(new PathString("/angular/module2/"), out newPathRequest));
      Assert.Equal("/embedded-path/module2", newPathRequest);
    }
  }
}
