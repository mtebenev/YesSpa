namespace YesSpa.Common
{
  /// <summary>
  /// Settings for a single hosted SPA
  /// </summary>
  public class SpaSettings
  {
    public SpaSettings(string rootUrlPath, string embeddedUrlRoot)
    {
      RootUrlPath = rootUrlPath;
      EmbeddedUrlRoot = embeddedUrlRoot;
    }

    /// <summary>
    /// Path for serving the SPA.
    /// For example a SPA should be accessible at www.server.com/features/clientapp
    /// The the property value should be 'features/clientapp'.
    /// Trailing and forwarding slashes are ignored
    /// </summary>
    public string RootUrlPath { get; }

    /// <summary>
    /// Root of embedded resource URL for SPA. This is module system-specific
    /// TODO MTE: revisit, do we need this? Provide requirements per each modular system
    /// </summary>
    public string EmbeddedUrlRoot { get; }
  }
}
