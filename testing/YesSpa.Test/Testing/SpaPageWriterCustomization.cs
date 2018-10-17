using AutoFixture;
using Microsoft.Extensions.Logging;
using YesSpa.Common;
using YesSpa.Common.Configuration;
using YesSpa.Common.StubPage;

namespace YesSpa.Test.Testing
{
  internal class SpaPageWriterCustomization : ICustomization
  {
    public SpaPageWriterCustomization()
    {
      UseStubPage = false;
      IsDevelopmentEnvironment = false;
    }
    public bool UseStubPage { get; set; }
    public bool IsDevelopmentEnvironment { get; set; }

    public void Customize(IFixture fixture)
    {
      fixture
        .Customize<SpaPageWriter>(c => c.FromFactory(
          () => new SpaPageWriter(
            fixture.Create<IYesSpaConfiguration>(),
            fixture.Create<IStubPageWriter>(),
            fixture.Create<ILogger>(),
            IsDevelopmentEnvironment,
            UseStubPage)));
    }

  }
}
