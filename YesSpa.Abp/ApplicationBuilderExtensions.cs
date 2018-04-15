using Abp.AspNetCore.EmbeddedResources;
using Abp.Dependency;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace YesSpa.Abp
{
  public static class ApplicationBuilderExtensions
  {
    public static void UseAbpSpa(this IApplicationBuilder applicationBuilder, string defaultPage)
    {
      var staticFileOptions = new StaticFileOptions
      {
        FileProvider = new EmbeddedResourceFileProvider(
          applicationBuilder.ApplicationServices.GetRequiredService<IIocResolver>())
      };

      applicationBuilder.UseSpaStaticFiles(staticFileOptions);

      applicationBuilder.UseSpa(builder =>
      {
        builder.Options.DefaultPageStaticFileOptions = staticFileOptions;
        builder.Options.DefaultPage = defaultPage;
      });
    }
  }
}
