using Microsoft.Extensions.DependencyInjection;

namespace YesSpa.Abp
{
  public static class ServiceCollectionExtensions
  {
    /// <summary>
    /// Call in Startup.ConfigureServices() to add YesSpa-specific services to application
    /// </summary>
    public static void AddYesSpa(this IServiceCollection services)
    {
      services.AddSingleton<IYesSpaAbpConfiguration, YesSpaAbpConfiguration>();
    }
  }
}
