using System.Linq;
using System.Reflection;
using Cortside.Common.BootStrap;
using Cortside.SqlReportApi.Facade;
using Cortside.SqlReportApi.Facade.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cortside.SqlReportApi.BootStrap.Installer {

    public class FacadeInstaller : IInstaller {

        public void Install(IServiceCollection services, IConfigurationRoot configuration) {
            typeof(ReportFacade).GetTypeInfo().Assembly.GetTypes()
                .Where(x => (x.Name.EndsWith("Facade"))
                    && x.GetTypeInfo().IsClass
                    && !x.GetTypeInfo().IsAbstract)
                .ToList()
                .ForEach(x => {
                    x.GetInterfaces().ToList()
                        .ForEach(i => services.AddScoped(i, x));
                });

            typeof(ReportMapper).GetTypeInfo().Assembly.GetTypes()
                .Where(x => (x.Name.EndsWith("Mapper"))
                    && x.GetTypeInfo().IsClass
                    && !x.GetTypeInfo().IsAbstract)
                .ToList()
                .ForEach(x => services.AddSingleton(x));
        }
    }
}
