using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Resources.Embedded;

namespace YesSpa.Abp
{
  public static class AbpConfigurationExtensions
  {
    public static void ConfigureSpa(this IAbpStartupConfiguration configuration, string rootPath, Assembly assembly, string resourceNamespace)
    {
      var resourceSet = new EmbeddedResourceSet(rootPath, assembly, resourceNamespace);
      configuration.EmbeddedResources.Sources.Add(resourceSet);
    }
  }
}
