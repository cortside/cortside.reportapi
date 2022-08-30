using Cortside.Common.BootStrap;
using Cortside.Common.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cortside.SqlReportApi.BootStrap.Installer {
    public class EncryptionInstaller : IInstaller {
        public void Install(IServiceCollection services, IConfigurationRoot configuration) {
            var secret = configuration["Encryption:Secret"];
            services.AddSingleton<IEncryptionService>(new EncryptionService(secret));
        }
    }
}
