using System;
using Microsoft.AspNetCore.Builder;

namespace YesSpa.Abp
{
  internal class YesSpaBuilder : IYesSpaBuilder
  {
    public IApplicationBuilder ApplicationBuilder { get; }

    public YesSpaOptions Options { get; }

    public YesSpaBuilder(IApplicationBuilder applicationBuilder, YesSpaOptions options)
    {
      ApplicationBuilder = applicationBuilder ?? throw new ArgumentNullException(nameof(applicationBuilder));
      Options = options ?? throw new ArgumentNullException(nameof(options));
    }
  }
}
