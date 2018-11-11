using Microsoft.AspNetCore.Http;

namespace YesSpa.Common.Configuration
{
  public interface IDefaultPageRewrite
  {
    /// <summary>
    /// Returns true if the request matches to an SPA URL, so need to rewrite to default index.html
    /// newRequestPath filled with the new request path (modular path)
    /// </summary>
    (bool matches, string newPath) MatchRequest(PathString requestPath);
  }
}
