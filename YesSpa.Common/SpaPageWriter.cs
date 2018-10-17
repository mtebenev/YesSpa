using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using YesSpa.Common.Configuration;
using YesSpa.Common.StubPage;

namespace YesSpa.Common
{
  /// <summary>
  /// Encapsulates logic for rewriting SPA HTTP requests to modular HTTP requests
  /// E.g. /angular -> /.Modules/AspNetCore.ClientApp.Angular/dist/aspnetcore-clientapp-angular/index.html
  /// </summary>
  public class SpaPageWriter
  {
    private readonly IReadOnlyList<DefaultPageRewrite> _defaultPageRewrites;
    private readonly bool _isDevelopmentEnvironment;
    private readonly bool _useStubPage;
    private readonly IStubPageWriter _stubPageWriter;
    private readonly ILogger _logger;

    private readonly Action<ILogger, string, bool, Exception> _logRequestMatch;

    public SpaPageWriter(IYesSpaConfiguration spaConfiguration, IStubPageWriter stubPageWriter, ILogger logger, bool isDevelopmentEnvironment, bool useStubPage)
    {
      _isDevelopmentEnvironment = isDevelopmentEnvironment;
      _useStubPage = useStubPage;
      _defaultPageRewrites = spaConfiguration.SpaDefaultPageRewrites;
      _stubPageWriter = stubPageWriter;
      _logger = logger;


      _logRequestMatch = LoggerMessage.Define<string, bool>(LogLevel.Debug, 1, "Matched request '{RequestPath}', result: {Result}");
    }

    /// <summary>
    /// Looks for matched modular path and rewrites HTTP request if found
    /// Returns true if further middleware chain should be stopped
    /// </summary>
    public async Task<bool> TryRewriteSpaRequest(HttpContext context)
    {
      var result = false;

      string newRequestPath = null;
      var pageRewrite = _defaultPageRewrites.FirstOrDefault(r => r.MatchRequest(context.Request.Path, out newRequestPath));
      _logRequestMatch(_logger, context.Request.Path.Value, pageRewrite != null, null);
      if(pageRewrite != null)
      {
        // Rewrite URL in production, stub page in development environment
        if(_useStubPage && _isDevelopmentEnvironment)
        {
          await _stubPageWriter.WriteAsync(context);
          result = true;
        }
        else
        {
          context.Request.Path = newRequestPath; // Rewrite url, don't stop processing
        }
      }

      return result;
    }
  }
}
