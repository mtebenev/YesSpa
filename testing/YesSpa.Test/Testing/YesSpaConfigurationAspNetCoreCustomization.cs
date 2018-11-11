using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Moq;
using YesSpa.Common.Configuration;

namespace YesSpa.Test.Testing
{
  public class YesSpaConfigurationAspNetCoreCustomization : ICustomization
  {
    public YesSpaConfigurationAspNetCoreCustomization()
    {
      SpaSettings = new List<SpaSettings>();
      EmbeddedResources = new List<string>();
      RewritePaths = new Dictionary<string, string>();
    }

    public IList<SpaSettings> SpaSettings { get; set; }

    /// <summary>
    /// Optionally set up paths to embedded files
    /// </summary>
    public IList<string> EmbeddedResources { get; }

    /// <summary>
    /// Simulates default page rewriters (request path -> result paths)
    /// </summary>
    public Dictionary<string, string> RewritePaths { get; }

    public YesSpaConfigurationAspNetCoreCustomization WithEmbeddedResource(string path)
    {
      EmbeddedResources.Add(path);
      return this;
    }

    public YesSpaConfigurationAspNetCoreCustomization WithDefaultPageRewrite(string requestPath, string resultPath)
    {
      RewritePaths[requestPath] = resultPath;
      return this;
    }

    public void Customize(IFixture fixture)
    {
      fixture.Register<IYesSpaConfiguration>(() =>
      {
        var mockEmbeddedFileProvider = new Mock<IFileProvider>();
        mockEmbeddedFileProvider.Setup(x => x.GetFileInfo(It.IsAny<string>()))
          .Returns<string>(path =>
          {
            var mockFileInfo = new Mock<IFileInfo>();
            mockFileInfo.SetupGet(x => x.PhysicalPath).Returns(path);

            return EmbeddedResources.Any(s => s == path)
              ? mockFileInfo.Object
              : null;
          });

        var mockDefaultPageRewrite = new Mock<IDefaultPageRewrite>();
        mockDefaultPageRewrite.Setup(x => x.MatchRequest(It.IsAny<PathString>()))
          .Returns<PathString>(requestPath =>
          {
            return RewritePaths.ContainsKey(requestPath)
              ? (true, RewritePaths[requestPath])
              : (false, null);
          });

        var configuration = new Mock<IYesSpaConfiguration>();
        IList<IDefaultPageRewrite> defaultPageRewrites = new List<IDefaultPageRewrite> {mockDefaultPageRewrite.Object};
        configuration.Setup(x => x.CreateDefaultPageRewrites()).Returns(defaultPageRewrites);

        return configuration.Object;
      });
    }
  }
}
