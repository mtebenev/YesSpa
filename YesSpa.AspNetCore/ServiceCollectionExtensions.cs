using System;
using Microsoft.Extensions.DependencyInjection;
using YesSpa.Common.Configuration;

namespace YesSpa.AspNetCore
{
  public static class ServiceCollectionExtensions
  {
    /// <summary>
    /// Call in Startup.ConfigureServices() to add YesSpa-specific services to application
    /// SPAs should be registered in corresponding ABP modules
    /// </summary>
    public static IYesSpaBuilder AddYesSpa(this IServiceCollection services, Action<IYesSpaBuilder> builderCallback)
    {

      if(builderCallback == null)
        throw new ArgumentException(nameof(builderCallback));

      var spaBuilder = new YesSpaBuilderAspNetCore();
      builderCallback(spaBuilder);

      var yesSpaConfiguration = spaBuilder.BuildConfiguration();
      services.AddSingleton<IYesSpaConfiguration>(yesSpaConfiguration);

      return spaBuilder;
    }
  }
}
