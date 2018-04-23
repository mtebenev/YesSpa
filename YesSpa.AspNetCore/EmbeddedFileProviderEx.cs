using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using YesSpa.Common.Configuration;

namespace YesSpa.AspNetCore
{
  /// <summary>
  /// This custom <see cref="IFileProvider"/> implementation provides the file contents of embedded files in Asp.Net Core assemblies.
  /// Adapted from OrchardCore https://github.com/OrchardCMS/OrchardCore/blob/dev/src/OrchardCore/OrchardCore.Modules/ModuleEmbeddedFileProvider.cs
  /// </summary>
  internal class EmbeddedFileProviderEx : IFileProvider
  {
    public const string ModulesPath = ".Modules";
    public static string ModulesRoot = ModulesPath + "/";

    private readonly IList<ISpaModule> _spaModules;
    private readonly string _contentRoot;
    private readonly ILogger _logger;

    private readonly Action<ILogger, string, Exception> _logGetFileInfoStart;
    private readonly Action<ILogger, string, string, bool, Exception> _logModuleMatch;
    private readonly Action<ILogger, string, bool, Exception> _logGetFileInfoResult;

    public EmbeddedFileProviderEx(string contentPath, IList<ISpaModule> spaModules, ILoggerFactory loggerFactory)
    {
      _spaModules = spaModules;
      _contentRoot = contentPath != null ? NormalizePath(contentPath) + '/' : "";
      _logger = loggerFactory.CreateLogger<EmbeddedFileProviderEx>();

      _logGetFileInfoStart = LoggerMessage.Define<string>(LogLevel.Debug, 1, "GetFileInfo start, path: {Path}");
      _logModuleMatch = LoggerMessage.Define<string, string, bool>(LogLevel.Debug, 2, "GetFileInfo, matched module '{ModuleName}', subpath: '{SubPath}', result: {Result}");
      _logGetFileInfoResult = LoggerMessage.Define<string, bool>(LogLevel.Debug, 3, "GetFileInfo finished, path: {Path}, result: {Result}");

    }

    public IDirectoryContents GetDirectoryContents(string subpath)
    {
      // No need for files enumeration for the provider
      throw new NotImplementedException();
    }

    public IFileInfo GetFileInfo(string subpath)
    {
      _logGetFileInfoStart(_logger, subpath, null);

      IFileInfo fileInfo = null;
      if(subpath != null)
      {
        var path = _contentRoot + NormalizePath(subpath);

        if(path.StartsWith(ModulesRoot, StringComparison.Ordinal))
        {
          path = path.Substring(ModulesRoot.Length);
          var index = path.IndexOf('/');

          if(index != -1)
          {
            var moduleName = path.Substring(0, index);
            var fileSubPath = path.Substring(index + 1);

            var spaModule = _spaModules.FirstOrDefault(m => m.Name == moduleName);
            _logModuleMatch(_logger, moduleName, fileSubPath, spaModule != null, null);

            if(spaModule != null)
            {
              fileInfo = spaModule.GetFileInfo(fileSubPath);
            }
          }
        }
      }

      _logGetFileInfoResult(_logger, subpath, fileInfo != null, null);
      fileInfo = fileInfo ?? new NotFoundFileInfo(subpath);
      return fileInfo;
    }

    public IChangeToken Watch(string filter)
    {
      return NullChangeToken.Singleton;
    }

    private string NormalizePath(string path)
    {
      return path.Replace('\\', '/').Trim('/').Replace("//", "/");
    }
  }
}
