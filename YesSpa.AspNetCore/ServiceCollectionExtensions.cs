using Microsoft.Extensions.DependencyInjection;
using YesSpa.Common;

namespace YesSpa.AspNetCore
{
  public static class ServiceCollectionExtensions
  {
    /// <summary>
    /// Call in Startup.ConfigureServices() to add YesSpa-specific services to application
    /// SPAs should be registered in corresponding ABP modules
    /// </summary>
    public static void AddYesSpa(this IServiceCollection services)
    {
      services.AddSingleton<IYesSpaConfiguration, YesSpaConfigurationAspNetCore>();
    }
  }
}
