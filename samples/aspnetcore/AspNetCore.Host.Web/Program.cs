using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace YesSpa.Samples.AspNetCore.Host.Web
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
        .ConfigureLogging(((hostingContext, logging) =>
        {
          logging.AddConsole();
          logging.SetMinimumLevel(LogLevel.Debug);
        }))
        .UseStartup<Startup>();
  }
}
