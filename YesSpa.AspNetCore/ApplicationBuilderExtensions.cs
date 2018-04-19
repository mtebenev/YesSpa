using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using YesSpa.Common;

namespace YesSpa.AspNetCore
{
  public static class ApplicationBuilderExtensions
  {
    /// <summary>
    /// Use in Startup.Confiigure() to enable SPA routing. Use at the end of middleware chain.
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

      // Attach middleware
      var staticFileOptions = new StaticFileOptions
      {
        FileProvider = new EmbeddedFileProviderEx(null, spaBuilder.SpaModules.ToList())
      };

      var spaConfiguration = applicationBuilder.ApplicationServices.GetRequiredService<IYesSpaConfiguration>();

      foreach(SpaSettings settings in spaBuilder.SpaSettings)
      {
        spaConfiguration.AddSpa(settings.RootUrlPath, settings.EmbeddedUrlRoot);
      }

      SpaMiddleware.Attach(spaBuilder, staticFileOptions, spaConfiguration);
    }
  }
}
