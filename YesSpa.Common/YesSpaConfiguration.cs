using System.Collections.Generic;
using System.Linq;

namespace YesSpa.Common
{
  public class YesSpaConfiguration : IYesSpaConfiguration
  {
    private readonly List<SpaSettings> _spaSettings;

    public YesSpaConfiguration()
    {
      _spaSettings = new List<SpaSettings>();
    }

    public void AddSpa(string rootPath)
    {
      var settings = new SpaSettings(rootPath);
      _spaSettings.Add(settings);
    }

    public IReadOnlyList<SpaSettings> SpaSettings => _spaSettings;

    public IReadOnlyList<DefaultPageRewrite> SpaDefaultPageRewrites
    {
      get
      {
        var result = _spaSettings
          .Select(s =>
          {
            // Use convention (angular-cli and create-react-app use index.html
            var defaultPagePath = $"/{s.RootUrlPath.Trim('/')}/index.html";
            return new DefaultPageRewrite(s.RootUrlPath, defaultPagePath);
          })
          .ToList();
        return result;
      }
    }
  }
}
