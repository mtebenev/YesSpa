using System.Collections.Generic;

namespace YesSpa.Abp
{
  internal interface IYesSpaAbpConfiguration
  {
    /// <summary>
    /// Check SpaSettings for more info on parameters
    /// </summary>
    void AddSpa(string rootPath);

    IReadOnlyList<SpaSettings> SpaSettings { get; }
    IReadOnlyList<DefaultPageRewrite> SpaDefaultPageRewrites { get; }
  }
}
