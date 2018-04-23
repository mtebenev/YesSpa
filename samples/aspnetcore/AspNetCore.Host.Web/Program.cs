using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace YesSpa.Samples.AspNetCore.Host.Web
{
  public class Program
  {
    public static void Main(string[] args)
    {
      BuildWebHost(args).Run();
    }

    public static IWebHost BuildWebHost(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
        .ConfigureLogging(((hostingContext, logging) =>
        {
          logging.SetMinimumLevel(LogLevel.Debug);
        }))
        .UseStartup<Startup>()
        .Build();
  }
}
