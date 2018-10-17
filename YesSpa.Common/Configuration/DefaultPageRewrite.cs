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
  public class DefaultPageRewrite
  {
    public DefaultPageRewrite(string urlPath, string defaultPagePath, string indexPageFileName)
    {
      UrlPath = urlPath.Trim('/');
      DefaultPagePath = defaultPagePath;
      IndexPageFileName = indexPageFileName;
    }

    public string UrlPath { get; }
    public string DefaultPagePath { get; }
    public string IndexPageFileName { get; }

    /// <summary>
    /// Returns true if the request matches to an SPA URL, so need to rewrite to default index.html
    /// newRequestPath filled with the new request path (modular path)
    /// </summary>
    public bool MatchRequest(PathString requestPath, out string newRequestPath)
    {
      newRequestPath = null;
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
          newRequestPath = trimmedRequestPath.Length == UrlPath.Length
            ? $"{DefaultPagePath}/{IndexPageFileName}"
            : $"{DefaultPagePath}/{trimmedRequestPath.Substring(UrlPath.Length > 1 ? UrlPath.Length + 1 : 0)}"; // +1 for '/'
        }
      }

      return result;
    }
  }
}
