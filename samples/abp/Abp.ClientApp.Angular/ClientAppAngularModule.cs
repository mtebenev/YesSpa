using System.Reflection;
using Abp.Modules;
using YesSpa.Abp;

namespace YesSpa.Samples.Abp.ClientApp.Angular
{
  public class ClientAppAngularModule : AbpModule
  {
    public override void PreInitialize()
    {
      string rootPath = "/angular/";
      var assembly = Assembly.GetExecutingAssembly();
      var resourceNamespace = "YesSpa.Samples.Abp.ClientApp.Angular.dist";
      Configuration.ConfigureSpa(rootPath, resourceNamespace, assembly);
    }
  }
}
