using Microsoft.AspNetCore.Http;

namespace YesSpa.Common.Configuration
{
  /// <summary>
  /// Information necessary to rewrite URL to default SPA page (index.html)
  /// </summary>
  public class DefaultPageRewrite
  {
    public DefaultPageRewrite(string urlPath, string defaultPagePath)
    {
      UrlPath = urlPath.Trim('/');
      DefaultPagePath = defaultPagePath;
    }

    public string UrlPath { get; }
    public string DefaultPagePath { get; }

    /// <summary>
    /// Returns true if the request matches to an SPA URL, so need to rewrite to default index.html
    /// </summary>
    public bool IsMatching(PathString requestPath)
    {
      var result = UrlPath.EndsWith(requestPath.Value.Trim('/'));
      return result;
    }
  }
}
