using System.Collections.Generic;
using Cortside.Common.BootStrap;
using Cortside.SqlReportApi.BootStrap.Installer;

namespace Cortside.SqlReportApi.BootStrap {
    public class DefaultApplicationBootStrapper : BootStrapper {
        public DefaultApplicationBootStrapper() {
            installers = new List<IInstaller> {
                new HealthInstaller(),
                new DbContextInstaller(),
                //new RepositoryInstaller(),
                new DomainServiceInstaller(),
                new MiniProfilerInstaller(),
                new EncryptionInstaller()
                //new FacadeInstaller()
            };
        }
    }
}
