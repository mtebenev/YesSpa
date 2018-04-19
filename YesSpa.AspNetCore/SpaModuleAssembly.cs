using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Embedded;
using YesSpa.Common.Configuration;

namespace YesSpa.AspNetCore
{
  /// <summary>
  /// Implementation for AspNet.Core
  /// </summary>
  internal class SpaModuleAssembly : ISpaModule
  {
    private readonly string _name;
    private readonly string _baseNamespace;
    private readonly DateTimeOffset _lastModified;
    private readonly IDictionary<string, IFileInfo> _fileInfos;
    private readonly IAssemblyWrapper _assembly;

    public SpaModuleAssembly(IAssemblyWrapper assembly)
    {
      _name = assembly.GetName();
      _fileInfos = new Dictionary<string, IFileInfo>();
      _assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
      _baseNamespace = _name + '.';
      _lastModified = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// ISpaModule
    /// </summary>
    public string Name => _name;

    /// <summary>
    /// ISpaModule
    /// This implementation accepts path in Orchard-like format:
    /// 
    /// </summary>
    public IFileInfo GetFileInfo(string subpath)
    {
      if(!_fileInfos.TryGetValue(subpath, out var fileInfo))
      {
        lock(_fileInfos)
        {
          if(!_fileInfos.TryGetValue(subpath, out fileInfo))
          {
            var resourcePath = _baseNamespace + subpath.Replace('/', '>');
            var fileName = Path.GetFileName(subpath);

            if(_assembly.GetManifestResourceInfo(resourcePath) == null)
              return new NotFoundFileInfo(fileName);

            _fileInfos[subpath] = fileInfo = new EmbeddedResourceFileInfo(_assembly.Object, resourcePath, fileName, _lastModified);
          }
        }
      }

      return fileInfo;
    }
  }
}
