using System.Linq;
using System.Reflection;
using Cortside.Common.BootStrap;
using Cortside.SqlReportApi.Data.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cortside.SqlReportApi.BootStrap.Installer {

    public class RepositoryInstaller : IInstaller {

        public void Install(IServiceCollection services, IConfigurationRoot configuration) {
            // register repositories
            typeof(ReportRepository).GetTypeInfo().Assembly.GetTypes()
                .Where(x => (x.Name.EndsWith("Repository"))
                    && x.GetTypeInfo().IsClass
                    && !x.GetTypeInfo().IsAbstract
                    && x.GetInterfaces().Length > 0)
                .ToList().ForEach(x => {
                    x.GetInterfaces().ToList()
                        .ForEach(i => services.AddScoped(i, x));
                });
        }
    }
}
