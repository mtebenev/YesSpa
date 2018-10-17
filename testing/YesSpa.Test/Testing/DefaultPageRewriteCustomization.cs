using System;
using AutoFixture;
using YesSpa.Common.Configuration;

namespace YesSpa.Test.Testing
{
  internal class DefaultPageRewriteCustomization : ICustomization
  {
    public string RootUrlPath { get; set; }
    public string DefaultPagePath { get; set; }
    public void Customize(IFixture fixture)
    {
      fixture
        .Customize<DefaultPageRewrite>(c => c.FromFactory(
          () => new DefaultPageRewrite(
            RootUrlPath,
            String.IsNullOrEmpty(DefaultPagePath) ? fixture.Create<string>() : DefaultPagePath,
            "index.html")));
    }
  }
}
