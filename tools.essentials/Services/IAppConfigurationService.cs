using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace tools.Services
{
    public interface IAppConfigurationService
    {
        IConfigurationRoot Configuration { get; }
        IFileProvider WebRootFileProvider { get; }
        string WebRootPath { get; }
        bool IsDevelopment { get; }
        bool IsStaging { get; }
        bool IsProduction { get; }
    }
}
