using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using YesSpa.Common.Configuration;

namespace YesSpa.AspNetCore
{
  public static class ApplicationBuilderExtensions
  {
    /// <summary>
    /// Use in Startup.Configure() to enable SPA routing. Use at the end of middleware chain.
    /// </summary>
    public static void UseYesSpa(this IApplicationBuilder applicationBuilder)
    {
      var spaConfiguration = applicationBuilder.ApplicationServices.GetRequiredService<IYesSpaConfiguration>();
      spaConfiguration.UseYesSpa(applicationBuilder);
    }
  }
}
