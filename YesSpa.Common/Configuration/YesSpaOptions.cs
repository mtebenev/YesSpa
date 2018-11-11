namespace YesSpa.Common.Configuration
{
  /// <summary>
  /// Options common for all hosted SPAs
  /// </summary>
  public class YesSpaOptions
  {
    public YesSpaOptions()
    {
      UseStubPage = true;
    }

    /// <summary>
    /// If true, then YesSpa will render stub page instead of embedded SPA in development environment
    /// Enabled by default
    /// </summary>
    public bool UseStubPage { get; set; }
  }
}
