using Cortside.AspNetCore.Auditable;
using Cortside.AspNetCore.Auditable.Entities;
using Cortside.AspNetCore.EntityFramework;
using Cortside.Common.Security;
using Cortside.DomainEvent.EntityFramework;
using Cortside.SqlReportApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cortside.SqlReportApi.Data
{
    public class DatabaseContext : UnitOfWorkContext<Subject>, IDatabaseContext
    {
        public DatabaseContext(DbContextOptions options, ISubjectPrincipal subjectPrincipal, ISubjectFactory<Subject> subjectFactory) : base(options, subjectPrincipal, subjectFactory)
        {
        }

        public DbSet<ReportGroup> ReportGroups { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportArgument> ReportArguments { get; set; }
        public DbSet<ReportArgumentQuery> ReportArgumentQuerys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.AddDomainEventOutbox();

            SetDateTime(modelBuilder);
            SetCascadeDelete(modelBuilder);
        }
    }
}
