using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Microsoft.Extensions.FileProviders;
using Moq;
using YesSpa.AspNetCore;
using YesSpa.Common.Configuration;

namespace YesSpa.Test.Testing
{
  public class YesSpaConfigurationAspNetCoreCustomization : ICustomization
  {
    public YesSpaConfigurationAspNetCoreCustomization()
    {
      SpaSettings = new List<SpaSettings>();
      EmbeddedResources = new List<string>();
    }

    public IList<SpaSettings> SpaSettings { get; set; }

    /// <summary>
    /// Optionally set up paths to embedded files
    /// </summary>
    public IList<string> EmbeddedResources { get; }

    public YesSpaConfigurationAspNetCoreCustomization WithEmbeddedResource(string path)
    {
      EmbeddedResources.Add(path);
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

        var configuration = new YesSpaConfigurationAspNetCore(mockEmbeddedFileProvider.Object);
        foreach (var setting in SpaSettings)
        {
          configuration.AddSpa(setting.RootUrlPath, setting.EmbeddedUrlRoot);
        }

        return configuration;
      });
    }
  }
}
