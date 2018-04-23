using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YesSpa.Common.Configuration;

namespace YesSpa.AspNetCore
{
  internal class YesSpaBuilderAspNetCore : IYesSpaBuilder
  {
    private readonly List<ISpaModule> _spaModules;
    private readonly List<SpaSettings> _spaSettings;
    private readonly ILogger<SpaModuleAssembly> _logger;

    public YesSpaBuilderAspNetCore(IApplicationBuilder applicationBuilder, YesSpaOptions options)
    {
      _spaModules = new List<ISpaModule>();
      _spaSettings = new List<SpaSettings>();

      ApplicationBuilder = applicationBuilder ?? throw new ArgumentNullException(nameof(applicationBuilder));
      Options = options ?? throw new ArgumentNullException(nameof(options));
      var loggerFactory = applicationBuilder.ApplicationServices.GetRequiredService<ILoggerFactory>();
      _logger = loggerFactory.CreateLogger<SpaModuleAssembly>();
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
    public void AddSpa(Assembly assembly, string rootUrlPath, string embeddedUrlRoot)
    {
      var assemblyWrapper = new AssemblyWrapper(assembly);
      var spaModule = new SpaModuleAssembly(assemblyWrapper, _logger);
      _spaModules.Add(spaModule);
      _spaSettings.Add(new SpaSettings(rootUrlPath, embeddedUrlRoot));
    }

    public IReadOnlyList<ISpaModule> SpaModules => _spaModules;
    public IReadOnlyList<SpaSettings> SpaSettings => _spaSettings;
  }
}
