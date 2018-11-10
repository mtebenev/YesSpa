using System;
using AutoFixture;
using YesSpa.Common.Configuration;

namespace YesSpa.Test.Testing
{
  /// <summary>
  /// Helpers for fluent fixture customization
  /// </summary>
  public static class FixtureExtensions
  {
    /// <summary>
    /// Adds YesSpaConfiguration with single app mapping: '/angular/' -> '/.Modules/module/dist/app'
    /// </summary>
    public static IFixture WithYesSpaConfigurationAngular(this IFixture fixture, Action<YesSpaConfigurationAspNetCoreCustomization> cb = null)
    {
      var customization = new YesSpaConfigurationAspNetCoreCustomization();
      customization.SpaSettings.Add(new SpaSettings("/angular/", "/.Modules/module/dist/app"));

      cb?.Invoke(customization);
      fixture.Customize(customization);

      return fixture;
    }

    public static IFixture WithHttpContext(this IFixture fixture, string requestPath)
    {
      var customization = new HttpContextCustomization
      {
        RequestPath = requestPath
      };
      fixture.Customize(customization);

      return fixture;
    }
  }
}
