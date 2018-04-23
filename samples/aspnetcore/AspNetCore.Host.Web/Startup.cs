using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using YesSpa.AspNetCore;
using YesSpa.Samples.AspNetCore.ClientApp.Angular;
using YesSpa.Samples.AspNetCore.ClientApp.React;

namespace YesSpa.Samples.AspNetCore.Host.Web
{
  public class Startup
  {
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc();
      services.AddYesSpa();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if(env.IsDevelopment())
        app.UseDeveloperExceptionPage();

      app.UseMvc();
      app.UseSpa(builder =>
      {
        builder.AddSpa(typeof(ClientAppModuleReact).Assembly, "/react/", "/.Modules/AspNetCore.ClientApp.React/build");
        builder.AddSpa(typeof(ClientAppModuleAngular).Assembly, "/angular/", "/.Modules/AspNetCore.ClientApp.Angular/dist");
      });
    }
  }
}
