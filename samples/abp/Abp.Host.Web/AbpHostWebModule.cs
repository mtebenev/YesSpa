using Abp.AspNetCore;
using Abp.Modules;
using YesSpa.Samples.Abp.ClientApp.Angular;
using YesSpa.Samples.Abp.ClientApp.React;

namespace YesSpa.Samples.Abp.AbpHost.Web
{
  [DependsOn(
    typeof(AbpAspNetCoreModule),
    typeof(ClientAppReactModule),
    typeof(ClientAppAngularModule))]
  public class AbpHostWebModule : AbpModule
  {
  }
}
