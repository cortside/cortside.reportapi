using Microsoft.EntityFrameworkCore;
using webapi.Reporting.Entity;

namespace webapi.Reporting.Data {
    //    [DbConfigurationType(typeof(MyConfiguration))]
    public class MyContext : DbContext {
        public MyContext(DbContextOptions<MyContext> options) : base(options) {
        }

        public virtual DbSet<ReportGroup> ReportGroups { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<ReportArgument> ReportArguments { get; set; }
        public virtual DbSet<ReportArgumentQuery> ReportArgumentQuerys { get; set; }
    }
}
