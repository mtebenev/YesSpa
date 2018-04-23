using AutoFixture;
using Microsoft.Extensions.Logging;
using YesSpa.Common;
using YesSpa.Common.Configuration;
using YesSpa.Common.StubPage;

namespace YesSpa.Test.Testing
{
  internal class DefaultPageWriterCustomization : ICustomization
  {
    public DefaultPageWriterCustomization()
    {
      UseStubPage = false;
      IsDevelopmentEnvironment = false;
    }
    public bool UseStubPage { get; set; }
    public bool IsDevelopmentEnvironment { get; set; }

    public void Customize(IFixture fixture)
    {
      fixture
        .Customize<DefaultPageWriter>(c => c.FromFactory(
          () => new DefaultPageWriter(
            fixture.Create<IYesSpaConfiguration>(),
            fixture.Create<IStubPageWriter>(),
            fixture.Create<ILogger>(),
            IsDevelopmentEnvironment,
            UseStubPage)));
    }

  }
}
