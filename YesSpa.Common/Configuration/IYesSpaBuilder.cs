using System.Reflection;

namespace YesSpa.Common.Configuration
{
  /// <summary>
  /// Configure SPA options using the interface
  /// </summary>
  public interface IYesSpaBuilder
  {
    /// <summary>
    /// Run-time options for hosting SPA(s).
    /// </summary>
    YesSpaOptions Options { get; }

    /// <summary>
    /// Use to add another SPA configuration
    /// </summary>
    IYesSpaBuilder AddSpa(Assembly assembly, string rootUrlPath, string embeddedUrlRoot);
  }
}
