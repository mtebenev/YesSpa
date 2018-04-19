using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace YesSpa.Common.StubPage
{
  public class StubPageWriter : IStubPageWriter
  {
    public async Task WriteAsync(HttpContext context)
    {
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
