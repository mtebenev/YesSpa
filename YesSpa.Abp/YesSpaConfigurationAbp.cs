using YesSpa.Common.Configuration;

namespace YesSpa.Abp
{
  internal class YesSpaConfigurationAbp : YesSpaConfigurationBase
  {
    /// <summary>
    /// Follows ABP embedded provider URL format
    /// </summary>
    protected override DefaultPageRewrite GetDefaultPageRewrite(SpaSettings spaSettings)
    {
      var defaultPagePath = $"/{spaSettings.RootUrlPath.Trim('/')}/index.html";
      var result = new DefaultPageRewrite(spaSettings.RootUrlPath, defaultPagePath);

      return result;
    }
  }
}
