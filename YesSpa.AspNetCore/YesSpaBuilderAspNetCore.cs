using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using YesSpa.Common;

namespace YesSpa.AspNetCore
{
  public class YesSpaBuilderAspNetCore : IYesSpaBuilder
  {
    private readonly List<ISpaModule> _spaModules;

    public YesSpaBuilderAspNetCore(IApplicationBuilder applicationBuilder, YesSpaOptions options)
    {
      _spaModules = new List<ISpaModule>();
      ApplicationBuilder = applicationBuilder ?? throw new ArgumentNullException(nameof(applicationBuilder));
      Options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// IYesSpaBuilder
    /// </summary>
    public IApplicationBuilder ApplicationBuilder { get; }

    /// <summary>
    /// IYesSpaBuilder
    /// </summary>
    public YesSpaOptions Options { get; }

    /// <summary>
    /// IYesSpaBuilder
    /// </summary>
    public void AddSpa(Assembly assembly)
    {
      var assemblyWrapper = new AssemblyWrapper(assembly);
      var spaModule = new SpaModuleAssembly(assemblyWrapper);
      _spaModules.Add(spaModule);
    }

    public List<ISpaModule> SpaModules => _spaModules;
  }
}
