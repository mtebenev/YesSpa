using System.Collections.Generic;

namespace YesSpa.Common.Configuration
{
  public interface IYesSpaConfiguration
  {
    /// <summary>
    /// Check SpaSettings for more info on parameters
    /// </summary>
    void AddSpa(string rootUrlPath, string embeddedUrlRoot);

    IReadOnlyList<SpaSettings> SpaSettings { get; }
    IReadOnlyList<DefaultPageRewrite> SpaDefaultPageRewrites { get; }
  }
}