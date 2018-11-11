using Microsoft.Extensions.FileProviders;
using YesSpa.Common.Configuration;

namespace YesSpa.AspNetCore
{
  internal class DefaultPageRewriteAspNetCore : DefaultPageRewriteBase
  {
    private readonly IFileProvider _embeddedFileProvider;

    public DefaultPageRewriteAspNetCore(string urlPath, string defaultPagePath, string indexPageFileName, IFileProvider embeddedFileProvider) : base(urlPath, defaultPagePath, indexPageFileName)
    {
      _embeddedFileProvider = embeddedFileProvider;
    }

    protected override bool IsEmbeddedResourceExists(string path)
    {
      var fileInfo = _embeddedFileProvider.GetFileInfo(path);
      return fileInfo != null && fileInfo.Exists;
    }
  }
}
