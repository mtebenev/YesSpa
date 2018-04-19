using YesSpa.Common;

namespace YesSpa.AspNetCore
{
  internal class YesSpaConfigurationAspNetCore : YesSpaConfigurationBase
  {
    /// <summary>
    /// We use orchard-like modular system for asp.net core
    /// </summary>
    protected override DefaultPageRewrite GetDefaultPageRewrite(SpaSettings spaSettings)
    {
      var defaultPagePath = $"{spaSettings.EmbeddedUrlRoot.TrimEnd('/')}/index.html"; // for aspnetcore
      var result = new DefaultPageRewrite(spaSettings.RootUrlPath, defaultPagePath);

      return result;
    }
  }
}
