using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using YesSpa.Common;

namespace YesSpa.Abp
{
  internal class SpaMiddleware
  {
    public static void Attach(IYesSpaBuilder spaBuilder, StaticFileOptions staticFileOptions, IYesSpaConfiguration spaConfiguration)
    {
      if(spaBuilder == null)
        throw new ArgumentNullException(nameof(spaBuilder));

      var app = spaBuilder.ApplicationBuilder;
      var options = spaBuilder.Options;
      var defaultPageRewrites = spaConfiguration.SpaDefaultPageRewrites;

      // Rewrite requests to the default pages
      app.Use((context, next) =>
      {
        var pageRewrite = defaultPageRewrites.FirstOrDefault(r => r.IsMatching(context.Request.Path));
        if(pageRewrite != null)
          context.Request.Path = pageRewrite.DefaultPagePath;

        return next();
      });

      // Serve it as a static file
      // Developers who need to host more than one SPA with distinct default pages can
      // override the file provider
      app.UseStaticFiles(staticFileOptions);

      // If the default file didn't get served as a static file (usually because it was not
      // present on disk), the SPA is definitely not going to work.
      app.Use((context, next) =>
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