using System;
using Abp.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace YesSpa.Samples.Abp.AbpHost.Web
{
  public class Startup
  {
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
      services.AddMvc();
      var result = services.AddAbp<AbpHostWebModule>();

      return result;
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      app.UseAbp(); //Initializes ABP framework.

      if(env.IsDevelopment())
        app.UseDeveloperExceptionPage();

      app.UseMvc();
    }
  }
}
