using System.Collections.Generic;
using AutoFixture;
using YesSpa.AspNetCore;
using YesSpa.Common.Configuration;

namespace YesSpa.Test.Testing
{
  public class YesSpaConfigurationCustomization : ICustomization
  {
    public YesSpaConfigurationCustomization()
    {
      SpaSettings = new List<SpaSettings>();
    }

    public IList<SpaSettings> SpaSettings { get; set; }
    public void Customize(IFixture fixture)
    {
      fixture.Register<IYesSpaConfiguration>(() =>
      {
        var configuration = new YesSpaConfigurationAspNetCore();
        foreach (var setting in SpaSettings)
        {
          configuration.AddSpa(setting.RootUrlPath, setting.EmbeddedUrlRoot);
        }

        return configuration;
      });
    }
  }
}
