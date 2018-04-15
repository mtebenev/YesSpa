using Abp.AspNetCore;
using Abp.Modules;
using YesSpa.Samples.Abp.ClientApp.React;

namespace YesSpa.Samples.Abp.AbpHost.Web
{
  [DependsOn(
    typeof(AbpAspNetCoreModule),
    typeof(ClientAppReactModule))]
  public class AbpHostWebModule : AbpModule
  {
  }
}
