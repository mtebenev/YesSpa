using System;
using AutoFixture;
using Microsoft.Extensions.FileProviders;
using Moq;
using YesSpa.AspNetCore;

namespace YesSpa.Test.Testing
{
  internal class DefaultPageRewriteAspNetCoreCustomization : ICustomization
  {
    public string RootUrlPath { get; set; }
    public string DefaultPagePath { get; set; }

    public void Customize(IFixture fixture)
    {
      var mockEmbeddedFileProvider = new Mock<IFileProvider>();
      mockEmbeddedFileProvider.Setup(x => x.GetFileInfo(It.IsAny<string>())).Returns<IFileInfo>(null);

      fixture
        .Customize<DefaultPageRewriteAspNetCore>(c => c.FromFactory(
          () => new DefaultPageRewriteAspNetCore(
            RootUrlPath,
            String.IsNullOrEmpty(DefaultPagePath) ? fixture.Create<string>() : DefaultPagePath,
            "index.html",
            mockEmbeddedFileProvider.Object)));
    }
  }
}
