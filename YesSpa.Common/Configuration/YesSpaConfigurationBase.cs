using System.Collections.Generic;
using System.Linq;

namespace YesSpa.Common.Configuration
{
  public abstract class YesSpaConfigurationBase : IYesSpaConfiguration
  {
    private readonly List<SpaSettings> _spaSettings;

    protected YesSpaConfigurationBase()
    {
      _spaSettings = new List<SpaSettings>();
    }

    public void AddSpa(string rootUrlPath, string embeddedUrlRoot)
    {
      var settings = new SpaSettings(rootUrlPath, embeddedUrlRoot);
      _spaSettings.Add(settings);
    }

    public IReadOnlyList<IDefaultPageRewrite> SpaDefaultPageRewrites
    {
      get
      {
        var result = _spaSettings
          .Select(GetDefaultPageRewrite)
          .ToList();
        return result;
      }
    }

    /// <summary>
    /// Concrete implementation depends on modular system
    /// </summary>
    protected abstract IDefaultPageRewrite GetDefaultPageRewrite(SpaSettings spaSettings);
  }
}
