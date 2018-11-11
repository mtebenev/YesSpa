using System;
using System.Collections.Generic;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using YesSpa.AspNetCore;
using YesSpa.Common.Configuration;

namespace YesSpa.Test.AspNetCore
{
  public class YesSpaConfigurationAspNetCoreTest
  {
    [Fact]
    public void Create_Default_Page_Rewrite()
    {
      var fixture = new Fixture()
        .Customize(new AutoMoqCustomization { ConfigureMembers = true });

      fixture.Register<IReadOnlyList<SpaSettings>>(() => new[] { new SpaSettings("/angular", "/.Modules/embedded") });
      var sut = fixture.Create<YesSpaConfigurationAspNetCore>();

      var mockLoggerFactory = new Mock<ILoggerFactory>();
      var mockHostingEnvironment = new Mock<IHostingEnvironment>();

      var mockServiceProvider = new Mock<IServiceProvider>();
      mockServiceProvider.Setup(x => x.GetService(typeof(ILoggerFactory))).Returns(mockLoggerFactory.Object);
      mockServiceProvider.Setup(x => x.GetService(typeof(IHostingEnvironment))).Returns(mockHostingEnvironment.Object);

      var mockApplicationBuilder = new Mock<IApplicationBuilder>();
      mockApplicationBuilder.SetupGet(x => x.ApplicationServices).Returns(mockServiceProvider.Object);

      sut.UseYesSpa(mockApplicationBuilder.Object);
      var pageRewrites = sut.CreateDefaultPageRewrites();

      Assert.Equal(1, pageRewrites.Count);

      var matchResult = pageRewrites[0].MatchRequest(new PathString("/angular"));
      Assert.True(matchResult.matches);
    }
  }
}
