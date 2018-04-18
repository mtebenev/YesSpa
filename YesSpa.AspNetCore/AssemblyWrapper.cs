using System.Reflection;

namespace YesSpa.AspNetCore
{
  internal class AssemblyWrapper : IAssemblyWrapper
  {
    private readonly Assembly _assembly;

    public AssemblyWrapper(Assembly assembly)
    {
      _assembly = assembly;
    }

    public string GetName()
    {
      return _assembly.GetName().Name;
    }

    public ManifestResourceInfo GetManifestResourceInfo(string resourcePath)
    {
      return _assembly.GetManifestResourceInfo(resourcePath);
    }

    public Assembly Object => _assembly;
  }
}
