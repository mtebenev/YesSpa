using Microsoft.AspNetCore.Builder;

namespace YesSpa.Abp
{
  /// <summary>
  /// Configure SPA options using the interface
  /// </summary>
  public interface IYesSpaBuilder
  {
    /// <summary>
    /// The <see cref="IApplicationBuilder"/> representing the middleware pipeline
    /// in which the SPA is being hosted.
    /// </summary>
    IApplicationBuilder ApplicationBuilder { get; }

    /// <summary>
    /// Run-time options for hosting SPA(s).
    /// </summary>
    YesSpaOptions Options { get; }
  }
}
