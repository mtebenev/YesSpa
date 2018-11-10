using Microsoft.Extensions.FileProviders;
using YesSpa.Common.Configuration;

namespace YesSpa.AspNetCore
{
  internal class YesSpaConfigurationAspNetCore : YesSpaConfigurationBase
  {
    private readonly IFileProvider _embeddedFileProvider;

    public YesSpaConfigurationAspNetCore(IFileProvider embeddedFileProvider)
    {
      _embeddedFileProvider = embeddedFileProvider;
    }

    /// <summary>
    /// We use orchard-like modular system for asp.net core
    /// </summary>
    protected override IDefaultPageRewrite GetDefaultPageRewrite(SpaSettings spaSettings)
    {
      var defaultPagePath = spaSettings.EmbeddedUrlRoot.TrimEnd('/'); // for aspnetcore
      var result = new DefaultPageRewriteAspNetCore(spaSettings.RootUrlPath, defaultPagePath, "index.html", _embeddedFileProvider);

      return result;
    }
  }
}
