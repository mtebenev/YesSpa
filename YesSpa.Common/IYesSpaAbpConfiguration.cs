using System.Collections.Generic;

namespace YesSpa.Common
{
  public interface IYesSpaAbpConfiguration
  {
    /// <summary>
    /// Check SpaSettings for more info on parameters
    /// </summary>
    void AddSpa(string rootPath);

    IReadOnlyList<SpaSettings> SpaSettings { get; }
    IReadOnlyList<DefaultPageRewrite> SpaDefaultPageRewrites { get; }
  }
}
