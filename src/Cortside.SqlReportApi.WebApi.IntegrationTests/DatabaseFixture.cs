using System;
using Cortside.AspNetCore.Auditable.Entities;
using Cortside.SqlReportApi.Data;

namespace Cortside.SqlReportApi.WebApi.IntegrationTests {
    public static class DatabaseFixture {
        public static void SeedInMemoryDb(DatabaseContext dbContext) {
            var subject = new Subject(Guid.Empty, string.Empty, string.Empty, string.Empty, "system");
            dbContext.Subjects.Add(subject);

            // intentionally using this override to avoid the not implemented exception
            dbContext.SaveChanges(true);
        }
    }
}
