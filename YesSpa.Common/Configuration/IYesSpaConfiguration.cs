using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;

namespace YesSpa.Common.Configuration
{
  /// <summary>
  /// Run-time configuration for YesSpa
  /// </summary>
  public interface IYesSpaConfiguration
  {
    /// <summary>
    /// Run-time options for hosting SPA(s).
    /// </summary>
    YesSpaOptions Options { get; }

    IList<IDefaultPageRewrite> CreateDefaultPageRewrites();
    
    /// <summary>
    /// Call in Configure() stage to attach an SPA middleware
    /// </summary>
    void UseYesSpa(IApplicationBuilder applicationBuilder);
  }
}
