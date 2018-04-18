using System.Reflection;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using Xunit;
using YesSpa.AspNetCore;

namespace YesSpa.Test.AspNetCore
{
  public class SpaModuleAssemblyTest
  {
    [Fact]
    public void Get_FileInfo_With_Correct_Path()
    {
      var fixture = new Fixture()
          .Customize(new AutoMoqCustomization { ConfigureMembers = true});

      var assembly = fixture.Create<IAssemblyWrapper>();
      var expectedResourcePath = $"{assembly.GetName()}.build>index.html";

      var mockAssembly = Mock.Get(assembly);

      mockAssembly.Setup(x => x.GetManifestResourceInfo(It.IsIn(expectedResourcePath))).Returns(new ManifestResourceInfo(null, "any", new ResourceLocation()));
      mockAssembly.Setup(x => x.Object).Returns<Assembly>(null);

      var spaModule = new SpaModuleAssembly(assembly);
      var fileInfo = spaModule.GetFileInfo("build/index.html");

      Assert.Equal("index.html", fileInfo.Name);
      Assert.True(fileInfo.Exists);
    }

    [Fact]
    public void Get_Not_Found_Info()
    {
      var fixture = new Fixture()
        .Customize(new AutoMoqCustomization {ConfigureMembers = true});

      var assembly = fixture.Create<IAssemblyWrapper>();
      var expectedResourcePath = $"{assembly.GetName()}.build>index.html";

      var mockAssembly = Mock.Get(assembly);

      mockAssembly.Setup(x => x.GetManifestResourceInfo(It.IsAny<string>())).Returns<ManifestResourceInfo>(null);
      mockAssembly.Setup(x => x.Object).Returns<Assembly>(null);

      var spaModule = new SpaModuleAssembly(assembly);
      var fileInfo = spaModule.GetFileInfo("build/index.html");

      Assert.False(fileInfo.Exists);
    }
  }
}
