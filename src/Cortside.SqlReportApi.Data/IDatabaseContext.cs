using Cortside.SqlReportApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cortside.SqlReportApi.Data {
    public interface IDatabaseContext {
        DbSet<ReportGroup> ReportGroups { get; set; }
        DbSet<Report> Reports { get; set; }
        DbSet<ReportArgument> ReportArguments { get; set; }
        DbSet<ReportArgumentQuery> ReportArgumentQuerys { get; set; }
    }
}
