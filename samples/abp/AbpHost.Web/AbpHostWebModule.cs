using Abp.AspNetCore;
using Abp.Modules;

namespace YesSpa.Samples.Abp.AbpHost.Web
{
  [DependsOn(
    typeof(AbpAspNetCoreModule))]
  public class AbpHostWebModule : AbpModule
  {
  }
}
