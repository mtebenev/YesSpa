using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YesSpa.Common;
using YesSpa.Common.Configuration;
using YesSpa.Common.StubPage;

namespace YesSpa.AspNetCore
{
  internal class SpaMiddlewareAspNetCore
  {
    public static void Attach(IApplicationBuilder applicationBuilder, StaticFileOptions staticFileOptions, IYesSpaConfiguration spaConfiguration)
    {
      var options = spaConfiguration.Options;
      var hostingEnvironment = applicationBuilder.ApplicationServices.GetService<IHostingEnvironment>();
      var loggerFactory = applicationBuilder.ApplicationServices.GetRequiredService<ILoggerFactory>();
      var logger = loggerFactory.CreateLogger<SpaMiddlewareAspNetCore>();

      var stubPageWriter = new StubPageWriter(logger);
      var defaultPageWriter = new SpaPageWriter(spaConfiguration, stubPageWriter, logger, hostingEnvironment.IsDevelopment(), options.UseStubPage);

      // Rewrite requests to the default pages
      applicationBuilder.Use(async (context, next) =>
      {
        var shouldStop = await defaultPageWriter.TryRewriteSpaRequest(context);
        if(!shouldStop)
          await next();
      });

      // Serve it as a static file
      // Developers who need to host more than one SPA with distinct default pages can
      // override the file provider
      applicationBuilder.UseStaticFiles(staticFileOptions);

      // If the default file didn't get served as a static file (usually because it was not
      // present on disk), the SPA is definitely not going to work.
      applicationBuilder.Use((context, next) =>
      {
        var message = "The SPA default page middleware could not return the default page " +
                      $"because it was not found, and no other middleware " +
                      "handled the request.\n";

        // Try to clarify the common scenario where someone runs an application in
        // Production environment without first publishing the whole application
        // or at least building the SPA.
        var hostEnvironment = (IHostingEnvironment) context.RequestServices.GetService(typeof(IHostingEnvironment));
        if(hostEnvironment != null && hostEnvironment.IsProduction())
        {
          message += "Your application is running in Production mode, so make sure it has " +
                     "been published, or that you have built your SPA manually. Alternatively you " +
                     "may wish to switch to the Development environment.\n";
        }

        throw new InvalidOperationException(message);
      });
    }
  }
}
