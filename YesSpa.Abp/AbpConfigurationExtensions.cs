using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Resources.Embedded;
using YesSpa.Common.Configuration;

namespace YesSpa.Abp
{
  public static class AbpConfigurationExtensions
  {
    /// <summary>
    /// Call on module startup to configure embedded SPA
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="rootUrlPath"></param>
    /// <param name="resourceNamespace">Check https://www.aspnetboilerplate.com/Pages/Documents/Embedded-Resource-Files for details</param>
    /// <param name="assembly"></param>
    public static void ConfigureSpa(this IAbpStartupConfiguration configuration, string rootUrlPath, string resourceNamespace, Assembly assembly)
    {
      // Provide app configuration to YesSpa services
      var spaConfiguration = configuration.Get<IYesSpaConfiguration>();
      spaConfiguration.AddSpa(rootUrlPath, resourceNamespace);

      // Add embedded resource early to let Abp initialize embedded source
      var resourceSet = new EmbeddedResourceSet(rootUrlPath, assembly, resourceNamespace);
      configuration.EmbeddedResources.Sources.Add(resourceSet);
    }
  }
}
