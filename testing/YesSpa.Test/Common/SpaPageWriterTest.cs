using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Moq;
using Xunit;
using YesSpa.AspNetCore;
using YesSpa.Common;
using YesSpa.Common.Configuration;
using YesSpa.Common.StubPage;
using YesSpa.Test.Testing;

namespace YesSpa.Test.Common
{
  public class SpaPageWriterTest
  {
    [Fact]
    public async Task Should_Stop_After_Stub_Page()
    {
      var fixture = new Fixture()
        .Customize(new AutoMoqCustomization {ConfigureMembers = true})
        .Customize(new SpaPageWriterCustomization {IsDevelopmentEnvironment = true, UseStubPage = true})
        .WithHttpContext("/test-request");

      var mockEmbeddedFileProvider = new Mock<IFileProvider>();
      var mockSpaConfiguration = new Mock<IYesSpaConfiguration>();
      mockSpaConfiguration.SetupGet(x => x.SpaDefaultPageRewrites)
        .Returns(new List<DefaultPageRewriteAspNetCore> {new DefaultPageRewriteAspNetCore("/test-request", "/page-path", "index.html", mockEmbeddedFileProvider.Object)});

      fixture.Register(() => mockSpaConfiguration.Object);
      var httpContext = fixture.Create<HttpContext>();
      var sut = fixture.Create<SpaPageWriter>();

      bool result = await sut.TryRewriteSpaRequest(httpContext);

      Assert.True(result);
    }

    [Fact]
    public async Task Should_Proceed_In_Production()
    {
      var fixture = new Fixture()
        .Customize(new AutoMoqCustomization {ConfigureMembers = true})
        .Customize(new SpaPageWriterCustomization {IsDevelopmentEnvironment = false})
        .WithHttpContext("/test-request");

      var mockEmbeddedFileProvider = new Mock<IFileProvider>();
      var mockSpaConfiguration = fixture.Freeze<Mock<IYesSpaConfiguration>>();
      mockSpaConfiguration.SetupGet(x => x.SpaDefaultPageRewrites)
        .Returns(new List<DefaultPageRewriteAspNetCore> { new DefaultPageRewriteAspNetCore("/test-request", "/page-path", "index.html", mockEmbeddedFileProvider.Object) });

      var httpContext = fixture.Create<HttpContext>();
      var sut = fixture.Create<SpaPageWriter>();

      bool result = await sut.TryRewriteSpaRequest(httpContext);

      Assert.False(result);
    }

    [Fact]
    public async Task Use_Stub_Page()
    {
      var fixture = new Fixture()
        .Customize(new AutoMoqCustomization {ConfigureMembers = true})
        .Customize(new SpaPageWriterCustomization {IsDevelopmentEnvironment = false, UseStubPage = false})
        .WithHttpContext("/test-request");

      var mockStubPageWriter = fixture.Freeze<Mock<IStubPageWriter>>();
      var mockEmbeddedFileProvider = new Mock<IFileProvider>();
      var mockSpaConfiguration = fixture.Freeze<Mock<IYesSpaConfiguration>>();
      mockSpaConfiguration.SetupGet(x => x.SpaDefaultPageRewrites)
        .Returns(new List<DefaultPageRewriteAspNetCore> { new DefaultPageRewriteAspNetCore("/test-request", "/page-path", "index.html", mockEmbeddedFileProvider.Object) });

      var httpContext = fixture.Create<HttpContext>();
      var sut = fixture.Create<SpaPageWriter>();

      await sut.TryRewriteSpaRequest(httpContext);

      mockStubPageWriter.Verify(x => x.WriteAsync(It.IsAny<HttpContext>()), Times.Never);
    }

    /// <summary>
    /// If there's angular SPA configured for '/angular' path, should output 'index.html' path
    /// </summary>
    [Fact]
    public async Task Should_Output_Default_Spa_Page()
    {
      var fixture = new Fixture()
        .Customize(new AutoMoqCustomization { ConfigureMembers = true })
        .Customize(new SpaPageWriterCustomization { IsDevelopmentEnvironment = false, UseStubPage = false })
        .WithHttpContext("/angular")
        .WithYesSpaConfigurationAngular();

      var sut = fixture.Create<SpaPageWriter>();

      var httpContext = fixture.Create<HttpContext>();
      var shouldStop = await sut.TryRewriteSpaRequest(httpContext);

      Assert.False(shouldStop);
      Assert.Equal("/.Modules/module/dist/app/index.html", httpContext.Request.Path);
    }

    /// <summary>
    /// If there's angular SPA configured for '/angular' path and /angular/favico.ico' requested, should output modular path for favico.ico
    /// </summary>
    [Fact]
    public async Task Should_Output_Asset_File()
    {
      var fixture = new Fixture()
        .Customize(new AutoMoqCustomization { ConfigureMembers = true })
        .Customize(new SpaPageWriterCustomization { IsDevelopmentEnvironment = false, UseStubPage = false})
        .WithHttpContext("/angular/favicon.ico")
        .WithYesSpaConfigurationAngular(c => c.WithEmbeddedResource("/.Modules/module/dist/app/favicon.ico"));

      var sut = fixture.Create<SpaPageWriter>();

      var httpContext = fixture.Create<HttpContext>();
      var shouldStop = await sut.TryRewriteSpaRequest(httpContext);

      Assert.False(shouldStop);
      Assert.Equal("/.Modules/module/dist/app/favicon.ico", httpContext.Request.Path);
    }
  }
}
