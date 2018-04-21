namespace YesSpa.Common.Configuration
{
  /// <summary>
  /// Options common for all hosted SPAs
  /// </summary>
  public class YesSpaOptions
  {
    public YesSpaOptions()
    {
    }

    /// <summary>
    /// Use to copy settings from another instance
    /// </summary>
    public YesSpaOptions(YesSpaOptions copyFromOptions)
    {
      UseStubPage = copyFromOptions.UseStubPage;
    }

    /// <summary>
    /// If true, then YesSpa will render stub page instead of embedded SPA in development environment
    /// Enabled by default
    /// </summary>
    public bool UseStubPage { get; set; }
  }
}
