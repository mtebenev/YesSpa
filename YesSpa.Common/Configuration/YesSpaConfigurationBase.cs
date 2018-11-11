using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;

namespace YesSpa.Common.Configuration
{
  public abstract class YesSpaConfigurationBase : IYesSpaConfiguration
  {
    protected YesSpaConfigurationBase(YesSpaOptions options)
    {
      Options = options;
    }

    public YesSpaOptions Options { get; }

    /// <summary>
    /// Concrete implementation depends on modular system
    /// </summary>
    public abstract IList<IDefaultPageRewrite> CreateDefaultPageRewrites();
    public abstract void UseYesSpa(IApplicationBuilder applicationBuilder);
  }
}
