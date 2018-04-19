using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.FileProviders;
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

    public EmbeddedFileProviderEx(string contentPath, IList<ISpaModule> spaModules)
    {
      _spaModules = spaModules;
      _contentRoot = contentPath != null ? NormalizePath(contentPath) + '/' : "";
    }

    public IDirectoryContents GetDirectoryContents(string subpath)
    {
      // No need for files enumeration for the provider
      throw new NotImplementedException();
    }

    public IFileInfo GetFileInfo(string subpath)
    {
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
            if(spaModule != null)
            {
              fileInfo = spaModule.GetFileInfo(fileSubPath);
            }
          }
        }
      }

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
