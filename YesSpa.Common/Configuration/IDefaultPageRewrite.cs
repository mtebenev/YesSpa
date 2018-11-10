using Microsoft.AspNetCore.Http;

namespace YesSpa.Common.Configuration
{
  public interface IDefaultPageRewrite
  {
    string UrlPath { get; }
    string DefaultPagePath { get; }
    string IndexPageFileName { get; }

    /// <summary>
    /// Returns true if the request matches to an SPA URL, so need to rewrite to default index.html
    /// newRequestPath filled with the new request path (modular path)
    /// </summary>
    bool MatchRequest(PathString requestPath, out string newRequestPath);
  }
}
