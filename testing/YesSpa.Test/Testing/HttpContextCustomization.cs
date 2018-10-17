using AutoFixture;
using Microsoft.AspNetCore.Http;
using Moq;

namespace YesSpa.Test.Testing
{
  public class HttpContextCustomization : ICustomization
  {
    public string RequestPath { get; set; }

    public void Customize(IFixture fixture)
    {
      fixture.Customize<HttpContext>(c =>
        c.FromFactory(() =>
        {
          var mockRequest = new Mock<HttpRequest>();
          mockRequest.SetupProperty(x => x.Path, new PathString(RequestPath));

          var mockResponse = new Mock<HttpResponse>();

          var mockContext = new Mock<HttpContext>();
          mockContext.SetupGet(x => x.Request).Returns(mockRequest.Object);
          mockContext.SetupGet(x => x.Response).Returns(mockResponse.Object);
          return mockContext.Object;
        }));
    }
  }
}
