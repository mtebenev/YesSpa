using System.Reflection;

namespace YesSpa.AspNetCore
{
  /// <summary>
  /// Wrapper interface to allow mocking
  /// </summary>
  internal interface IAssemblyWrapper
  {
    string GetName();
    ManifestResourceInfo GetManifestResourceInfo(string resourcePath);
    Assembly Object { get; }
  }
}
