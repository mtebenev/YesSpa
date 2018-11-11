using System;
using Microsoft.AspNetCore.Http;

namespace YesSpa.Common.Configuration
{
  /// <summary>
  /// Information necessary to rewrite URL to default SPA page (index.html)
  /// urlPath - root path exposed on the web server as SPA path ('/angular')
  /// defaultPagePath - embedded path to SPA root  ('/.Modules/module/dist/app')
  /// indexPageFileName - file name for index page ('index.html')
  /// </summary>
  public abstract class DefaultPageRewriteBase : IDefaultPageRewrite
  {
    protected DefaultPageRewriteBase(string urlPath, string defaultPagePath, string indexPageFileName)
    {
      UrlPath = urlPath.Trim('/');
      DefaultPagePath = defaultPagePath;
      IndexPageFileName = indexPageFileName;
    }

    public string UrlPath { get; }
    public string DefaultPagePath { get; }
    public string IndexPageFileName { get; }

    public (bool matches, string newPath) MatchRequest(PathString requestPath)
    {
      string newRequestPath = null;
      var trimmedRequestPath = requestPath.Value.Trim('/');
      bool result = false;

      if(String.IsNullOrEmpty(trimmedRequestPath))
      {
        if(String.IsNullOrEmpty(UrlPath))
        {
          result = true;
          newRequestPath = $"{DefaultPagePath}/{IndexPageFileName}";
        }
      }
      else
      {
        if(trimmedRequestPath.StartsWith(UrlPath))
        {
          result = true;
          string embeddedResourcePath = "";

          if(trimmedRequestPath.Length > UrlPath.Length)
            embeddedResourcePath = $"{DefaultPagePath}/{trimmedRequestPath.Substring(UrlPath.Length > 1 ? UrlPath.Length + 1 : 0)}"; // +1 for '/'

          // For nested (actually client paths) we check if the actual file exists (embedded resource like icon)
          newRequestPath = (trimmedRequestPath.Length == UrlPath.Length) || !IsEmbeddedResourceExists(embeddedResourcePath)
            ? $"{DefaultPagePath}/{IndexPageFileName}"
            : embeddedResourcePath;
        }
      }

      return (result, newRequestPath);
    }

    /// <summary>
    /// Override in derived class. Should check if a given embedded resource exists
    /// </summary>
    /// <param name="path">Embedded resource path</param>
    /// <returns>True if the resource exists</returns>
    protected abstract bool IsEmbeddedResourceExists(string path);
  }
}
