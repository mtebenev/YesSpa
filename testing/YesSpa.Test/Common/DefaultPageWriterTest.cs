using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;
using YesSpa.Common;
using YesSpa.Common.Configuration;
using YesSpa.Common.StubPage;
using YesSpa.Test.Testing;

namespace YesSpa.Test.Common
{

  public class HttpContextCustomization : ICustomization
  {
    public string RequestPath { get; set; }

    public void Customize(IFixture fixture)
    {
      fixture.Customize<HttpContext>(c =>
        c.FromFactory(() =>
        {
          var mockRequest = new Mock<HttpRequest>();
          mockRequest.SetupGet(x => x.Path).Returns(new PathString(RequestPath));

          var mockResponse = new Mock<HttpResponse>();

          var mockContext = new Mock<HttpContext>();
          mockContext.SetupGet(x => x.Request).Returns(mockRequest.Object);
          mockContext.SetupGet(x => x.Response).Returns(mockResponse.Object);
          return mockContext.Object;
        }));
    }
  }

  public class DefaultPageWriterTest
  {
    [Fact]
    public async Task Should_Stop_After_Stub_Page()
    {
      var fixture = new Fixture()
        .Customize(new AutoMoqCustomization {ConfigureMembers = true})
        .Customize(new DefaultPageWriterCustomization {IsDevelopmentEnvironment = true, UseStubPage = true})
        .Customize(new HttpContextCustomization {RequestPath = "/test-request"});

      var mockSpaConfiguration = new Mock<IYesSpaConfiguration>();
      mockSpaConfiguration.SetupGet(x => x.SpaDefaultPageRewrites)
        .Returns(new List<DefaultPageRewrite> {new DefaultPageRewrite("/test-request", "/page-path")});

      fixture.Register(() => mockSpaConfiguration.Object);
      var httpContext = fixture.Create<HttpContext>();
      var sut = fixture.Create<DefaultPageWriter>();

      bool result = await sut.WriteDefaultPage(httpContext);

      Assert.True(result);
    }

    [Fact]
    public async Task Should_Proceed_In_Production()
    {
      var fixture = new Fixture()
        .Customize(new AutoMoqCustomization {ConfigureMembers = true})
        .Customize(new DefaultPageWriterCustomization {IsDevelopmentEnvironment = false})
        .Customize(new HttpContextCustomization {RequestPath = "/test-request"});

      var mockSpaConfiguration = fixture.Freeze<Mock<IYesSpaConfiguration>>();
      mockSpaConfiguration.SetupGet(x => x.SpaDefaultPageRewrites)
        .Returns(new List<DefaultPageRewrite> {new DefaultPageRewrite("/test-request", "/page-path")});

      var httpContext = fixture.Create<HttpContext>();
      var sut = fixture.Create<DefaultPageWriter>();

      bool result = await sut.WriteDefaultPage(httpContext);

      Assert.False(result);
    }

    [Fact]
    public async Task Use_Stub_Page()
    {
      var fixture = new Fixture()
        .Customize(new AutoMoqCustomization {ConfigureMembers = true})
        .Customize(new DefaultPageWriterCustomization {IsDevelopmentEnvironment = false, UseStubPage = false})
        .Customize(new HttpContextCustomization {RequestPath = "/test-request"});

      var mockStubPageWriter = fixture.Freeze<Mock<IStubPageWriter>>();
      var mockSpaConfiguration = fixture.Freeze<Mock<IYesSpaConfiguration>>();
      mockSpaConfiguration.SetupGet(x => x.SpaDefaultPageRewrites)
        .Returns(new List<DefaultPageRewrite> {new DefaultPageRewrite("/test-request", "/page-path")});

      var httpContext = fixture.Create<HttpContext>();
      var sut = fixture.Create<DefaultPageWriter>();

      await sut.WriteDefaultPage(httpContext);

      mockStubPageWriter.Verify(x => x.WriteAsync(It.IsAny<HttpContext>()), Times.Never);
    }
  }
}
