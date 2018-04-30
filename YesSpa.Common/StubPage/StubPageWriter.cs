using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace YesSpa.Common.StubPage
{
  public class StubPageWriter : IStubPageWriter
  {
    private readonly ILogger _logger;

    public StubPageWriter(ILogger logger)
    {
      _logger = logger;
    }

    public async Task WriteAsync(HttpContext context)
    {
      _logger.LogDebug(1, "Rendering SPA stubpage");

      context.Response.ContentType = "text/html; charset=utf-8";
      context.Response.ContentLength = null; // Clear any prior Content-Length
      
      var assembly = Assembly.GetExecutingAssembly();
      using(var resourceStream = assembly.GetManifestResourceStream("YesSpa.Common.StubPage.StubPage.html"))
      {
        using(var reader = new StreamReader(resourceStream))
        {
          var html = await reader.ReadToEndAsync();
          await context.Response.WriteAsync(html);
        }
      }
    }
  }
}
