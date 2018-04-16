namespace YesSpa.Abp
{
  /// <summary>
  /// Settings for a single hosted SPA
  /// </summary>
  internal class SpaSettings
  {
    public SpaSettings(string rootUrlPath)
    {
      RootUrlPath = rootUrlPath;
    }

    /// <summary>
    /// Path for serving the SPA.
    /// For example a SPA should be accessible at www.server.com/features/clientapp
    /// The the property value should be 'features/clientapp'.
    /// Trailing and forwarding slashes are ignored
    /// </summary>
    public string RootUrlPath { get; }
  }
}
