using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace YesSpa.Common.StubPage
{
  /// <summary>
  /// Used to output a stub page instead of SPA when it's not embedded (development environment)
  /// </summary>
  public interface IStubPageWriter
  {
    Task WriteAsync(HttpContext context);
  }
}
