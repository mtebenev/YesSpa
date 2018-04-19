using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using YesSpa.Common;

namespace YesSpa.Abp
{
  internal class YesSpaBuilderAbp : IYesSpaBuilder
  {
    public YesSpaBuilderAbp(IApplicationBuilder applicationBuilder, YesSpaOptions options)
    {
      ApplicationBuilder = applicationBuilder ?? throw new ArgumentNullException(nameof(applicationBuilder));
      Options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public IApplicationBuilder ApplicationBuilder { get; }
    public YesSpaOptions Options { get; }
    public void AddSpa(Assembly assembly, string rootUrlPath, string embeddedUrlRoot)
    {
      // Not used in ABP
      // TODO MTE: revisit interface
      throw new System.NotImplementedException();
    }
  }
}
