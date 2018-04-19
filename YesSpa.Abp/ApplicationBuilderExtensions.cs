using System;
using Abp.AspNetCore.EmbeddedResources;
using Abp.Dependency;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using YesSpa.Common;
using YesSpa.Common.Configuration;

namespace YesSpa.Abp
{
  public static class ApplicationBuilderExtensions
  {
    /// <summary>
    /// Use in Startup.Confiigure() to enable SPA routing. Use at the end of middleware chain.
    /// </summary>
    internal static void UseAbpSpa(this IApplicationBuilder applicationBuilder, Action<IYesSpaBuilder> configuration)
    {
      if(configuration == null)
        throw new ArgumentNullException(nameof(configuration));

      // Build options
      var optionsProvider = applicationBuilder.ApplicationServices.GetService<IOptions<YesSpaOptions>>();
      var options = new YesSpaOptions(optionsProvider.Value);
      var spaBuilder = new YesSpaBuilderAbp(applicationBuilder, options);
      configuration.Invoke(spaBuilder);

      // Attach middleware
      var staticFileOptions = new StaticFileOptions
      {
        FileProvider = new EmbeddedResourceFileProvider(
          applicationBuilder.ApplicationServices.GetRequiredService<IIocResolver>())
      };

      var spaConfiguration = applicationBuilder.ApplicationServices.GetRequiredService<IYesSpaConfiguration>();
      SpaMiddleware.Attach(spaBuilder, staticFileOptions, spaConfiguration);
    }

    /// <summary>
    /// Shorthand for using a default configuration
    /// </summary>
    public static void UseAbpSpa(this IApplicationBuilder applicationBuilder)
    {
      applicationBuilder.UseAbpSpa(builder => { });
    }
  }
}
