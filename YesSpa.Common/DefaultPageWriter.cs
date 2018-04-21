using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

    public DefaultPageWriter(IYesSpaConfiguration spaConfiguration, IStubPageWriter stubPageWriter, bool isDevelopmentEnvironment, bool useStubPage)
    {
      _isDevelopmentEnvironment = isDevelopmentEnvironment;
      _useStubPage = useStubPage;
      _defaultPageRewrites = spaConfiguration.SpaDefaultPageRewrites;
      _stubPageWriter = stubPageWriter;
    }

    /// <summary>
    /// Returns true if further middleware chain should be stopped
    /// </summary>
    public async Task<bool> WriteDefaultPage(HttpContext context)
    {
      var result = false;
      var pageRewrite = _defaultPageRewrites.FirstOrDefault(r => r.IsMatching(context.Request.Path));
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
