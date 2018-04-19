using System.Reflection;
using Abp.Modules;
using YesSpa.Abp;

namespace YesSpa.Samples.Abp.ClientApp.React
{
  public class ClientAppReactModule : AbpModule
  {
    public override void PreInitialize()
    {
      string rootPath = "/react/";
      var assembly = Assembly.GetExecutingAssembly();
      var resourceNamespace = "YesSpa.Samples.Abp.ClientApp.React.build";
      Configuration.ConfigureSpa(rootPath, resourceNamespace, assembly);
    }
  }
}
