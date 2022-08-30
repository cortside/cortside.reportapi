using System;
using System.Collections.Generic;
using Cortside.AspNetCore.Auditable;
using Cortside.Common.Security;
using Cortside.SqlReportApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Cortside.SqlReportApi.WebApi.Tests {
    public class UnitTestFixture {
        private readonly List<Mock> mocks;
        protected readonly Mock<IHttpContextAccessor> httpContextAccessorMock = new Mock<IHttpContextAccessor>();

        public UnitTestFixture() {
            mocks = new List<Mock>();
        }

        public Mock<T> Mock<T>() where T : class {
            var mock = new Mock<T>();
            mocks.Add(mock);
            return mock;
        }

        public DatabaseContext GetDatabaseContext() {
            var databaseContextOptions = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase($"db-{Guid.NewGuid():d}").Options;
            return new DatabaseContext(databaseContextOptions, SubjectPrincipal.From(httpContextAccessorMock.Object.HttpContext.User), new DefaultSubjectFactory());
        }

        public void TearDown() {
            mocks.ForEach(m => m.VerifyAll());
        }
    }
}
