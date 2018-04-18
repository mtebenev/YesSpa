using Microsoft.Extensions.FileProviders;

namespace YesSpa.Common
{
  /// <summary>
  /// Abstract interface for accessing embedded files.
  /// Adapters can utilize Orchard or Abp 
  /// </summary>
  public interface ISpaModule
  {
    string Name { get; }
    IFileInfo GetFileInfo(string subpath);
  }
}
