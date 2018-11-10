using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using YesSpa.Common.Configuration;

namespace YesSpa.AspNetCore
{
  public static class ApplicationBuilderExtensions
  {
    /// <summary>
    /// Use in Startup.Configure() to enable SPA routing. Use at the end of middleware chain.
    /// </summary>
    public static void UseSpa(this IApplicationBuilder applicationBuilder, Action<IYesSpaBuilder> configuration)
    {
      if(configuration == null)
        throw new ArgumentNullException(nameof(configuration));

      // Build options
      var optionsProvider = applicationBuilder.ApplicationServices.GetService<IOptions<YesSpaOptions>>();
      var options = new YesSpaOptions(optionsProvider.Value);
      var spaBuilder = new YesSpaBuilderAspNetCore(applicationBuilder, options);
      configuration.Invoke(spaBuilder);
      var loggerFactory = applicationBuilder.ApplicationServices.GetRequiredService<ILoggerFactory>();

      // Attach middleware
      var staticFileOptions = new StaticFileOptions
      {
        FileProvider = new EmbeddedFileProviderEx(null, spaBuilder.SpaModules.ToList(), loggerFactory)
      };

      var spaConfiguration = applicationBuilder.ApplicationServices.GetRequiredService<IYesSpaConfiguration>();

      foreach(SpaSettings settings in spaBuilder.SpaSettings)
      {
        spaConfiguration.AddSpa(settings.RootUrlPath, settings.EmbeddedUrlRoot);
      }

      SpaMiddlewareAspNetCore.Attach(spaBuilder, staticFileOptions, spaConfiguration);
    }
  }
}
