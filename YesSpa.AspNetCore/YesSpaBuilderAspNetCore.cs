using System.Collections.Generic;
using System.Reflection;
using YesSpa.Common.Configuration;

namespace YesSpa.AspNetCore
{
  internal class YesSpaBuilderAspNetCore : IYesSpaBuilder
  {
    private readonly List<IAssemblyWrapper> _spaAssemblies;
    private readonly List<SpaSettings> _spaSettings;

    public YesSpaBuilderAspNetCore()
    {
      _spaAssemblies = new List<IAssemblyWrapper>();
      _spaSettings = new List<SpaSettings>();
      Options = new YesSpaOptions();
    }

    /// <summary>
    /// IYesSpaBuilder
    /// </summary>
    public YesSpaOptions Options { get; }

    /// <summary>
    /// IYesSpaBuilder
    /// </summary>
    public IYesSpaBuilder AddSpa(Assembly assembly, string rootUrlPath, string embeddedUrlRoot)
    {
      var assemblyWrapper = new AssemblyWrapper(assembly);
      _spaAssemblies.Add(assemblyWrapper);
      _spaSettings.Add(new SpaSettings(rootUrlPath, embeddedUrlRoot));

      return this;
    }

    public IYesSpaConfiguration BuildConfiguration()
    {
      var configuration = new YesSpaConfigurationAspNetCore(_spaAssemblies, _spaSettings, Options);
      return configuration;
    }
  }
}
