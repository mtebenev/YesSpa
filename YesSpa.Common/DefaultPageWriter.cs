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
  public class DefaultPageWriter
  {
    private readonly IReadOnlyList<DefaultPageRewrite> _defaultPageRewrites;
    private readonly bool _isDevelopmentEnvironment;
    private readonly bool _useStubPage;
    private readonly IStubPageWriter _stubPageWriter;
    private readonly ILogger _logger;

    private readonly Action<ILogger, string, bool, Exception> _logRequestMatch;

    public DefaultPageWriter(IYesSpaConfiguration spaConfiguration, IStubPageWriter stubPageWriter, ILogger logger, bool isDevelopmentEnvironment, bool useStubPage)
    {
      _isDevelopmentEnvironment = isDevelopmentEnvironment;
      _useStubPage = useStubPage;
      _defaultPageRewrites = spaConfiguration.SpaDefaultPageRewrites;
      _stubPageWriter = stubPageWriter;
      _logger = logger;


      _logRequestMatch = LoggerMessage.Define<string, bool>(LogLevel.Debug, 1, "Matched request '{RequestPath}', result: {Result}");
    }

    /// <summary>
    /// Returns true if further middleware chain should be stopped
    /// </summary>
    public async Task<bool> WriteDefaultPage(HttpContext context)
    {
      var result = false;

      var pageRewrite = _defaultPageRewrites.FirstOrDefault(r => r.IsMatching(context.Request.Path));
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
          context.Request.Path = pageRewrite.DefaultPagePath; // Rewrite url, don't stop processing
        }
      }

      return result;
    }
  }
}
