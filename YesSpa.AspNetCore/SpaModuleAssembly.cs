using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Embedded;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger _logger;

    public SpaModuleAssembly(IAssemblyWrapper assembly, ILogger logger)
    {
      _assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
      _logger = logger;

      _name = assembly.GetName();
      _fileInfos = new Dictionary<string, IFileInfo>();
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
            {
              _logger.LogDebug(1, null, $"SpaModuleAssembly.GetFileInfo(): cannot find resource '{subpath}'");
              fileInfo = new NotFoundFileInfo(fileName);
            }
            else
            {
              _logger.LogDebug(2, null, $"SpaModuleAssembly.GetFileInfo(): successfully loaded resource '{subpath}'");
              _fileInfos[subpath] = fileInfo = new EmbeddedResourceFileInfo(_assembly.Object, resourcePath, fileName, _lastModified);
            }
          }
        }
      }

      return fileInfo;
    }
  }
}
