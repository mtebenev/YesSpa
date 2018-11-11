using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using YesSpa.Common.Configuration;

namespace YesSpa.AspNetCore
{
  internal class YesSpaConfigurationAspNetCore : YesSpaConfigurationBase
  {
    private readonly IList<IAssemblyWrapper> _spaAssemblies;
    private readonly IList<SpaSettings> _spaSettings;
    private IFileProvider _embeddedFileProvider;

    public YesSpaConfigurationAspNetCore(IReadOnlyList<IAssemblyWrapper> spaAssemblies, IReadOnlyList<SpaSettings> spaSettings, YesSpaOptions options)
      : base(options)
    {
      _spaAssemblies = spaAssemblies.ToList();
      _spaSettings = spaSettings.ToList();
    }

    public override IList<IDefaultPageRewrite> CreateDefaultPageRewrites()
    {
      var result = _spaSettings
        .Select(ss => CreateDefaultPageRewrite(ss))
        .ToList();

      return result;
    }

    /// <summary>
    /// IYesSpaConfiguration
    /// </summary>
    public override void UseYesSpa(IApplicationBuilder applicationBuilder)
    {
      var loggerFactory = applicationBuilder.ApplicationServices.GetRequiredService<ILoggerFactory>();

      // Init embedded file provider
      var spaAssemblyLogger = loggerFactory.CreateLogger<SpaModuleAssembly>();
      var spaModules = _spaAssemblies.Select(a => new SpaModuleAssembly(a, spaAssemblyLogger) as ISpaModule).ToList();

      _embeddedFileProvider = new EmbeddedFileProviderEx(null, spaModules, loggerFactory);

      // Attach middleware
      var staticFileOptions = new StaticFileOptions
      {
        FileProvider = _embeddedFileProvider
      };

      SpaMiddlewareAspNetCore.Attach(applicationBuilder, staticFileOptions, this);
    }
    
    /// <summary>
    /// We use orchard-like modular system for asp.net core
    /// </summary>
    private IDefaultPageRewrite CreateDefaultPageRewrite(SpaSettings spaSettings)
    {
      if(_embeddedFileProvider == null)
        throw new InvalidOperationException("Unexpected call to CreateDefaultPageRewrite(). Did you call UseYesSpa() first?");

      var defaultPagePath = spaSettings.EmbeddedUrlRoot.TrimEnd('/'); // for aspnetcore
      var result = new DefaultPageRewriteAspNetCore(spaSettings.RootUrlPath, defaultPagePath, "index.html", _embeddedFileProvider);

      return result;
    }
  }
}
