using YesSpa.Common.Configuration;

namespace YesSpa.AspNetCore
{
  internal class YesSpaConfigurationAspNetCore : YesSpaConfigurationBase
  {
    /// <summary>
    /// We use orchard-like modular system for asp.net core
    /// </summary>
    protected override DefaultPageRewrite GetDefaultPageRewrite(SpaSettings spaSettings)
    {
      var defaultPagePath = spaSettings.EmbeddedUrlRoot.TrimEnd('/'); // for aspnetcore
      var result = new DefaultPageRewrite(spaSettings.RootUrlPath, defaultPagePath, "index.html");

      return result;
    }
  }
}
