using Cortside.Common.BootStrap;
using Cortside.Common.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Cortside.SqlReportApi.WebApi.Installers {
    public class NewtonsoftInstaller : IInstaller {
        public void Install(IServiceCollection services, IConfigurationRoot configuration) {
            JsonConvert.DefaultSettings = () => {
                var settings = new JsonSerializerSettings {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                settings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
                settings.Converters.Add(new IsoTimeSpanConverter());

                settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                settings.MissingMemberHandling = MissingMemberHandling.Ignore;
                settings.NullValueHandling = NullValueHandling.Include;
                settings.DefaultValueHandling = DefaultValueHandling.Include;

                settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                settings.DateParseHandling = DateParseHandling.DateTimeOffset;

                return settings;
            };
        }
    }
}
