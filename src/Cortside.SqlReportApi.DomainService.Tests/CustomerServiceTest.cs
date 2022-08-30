//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using Cortside.SqlReportApi.Data;
//using Cortside.SqlReportApi.Data.Repositories;
//using Cortside.SqlReportApi.Dto;
//using Cortside.DomainEvent;
//using Microsoft.Extensions.Logging.Abstractions;
//using Moq;
//using Xunit;

//namespace Cortside.SqlReportApi.DomainService.Tests {
//    public class CustomerServiceTest : DomainServiceTest<ICustomerService> {
//        private readonly DatabaseContext databaseContext;

//        public CustomerServiceTest() : base() {
//            databaseContext = GetDatabaseContext();
//        }

//        [Fact]
//        public async Task ShouldCreateCustomerAsync() {
//            // Arrange
//            var dto = new CustomerDto() {
//                FirstName = Guid.NewGuid().ToString(),
//                LastName = Guid.NewGuid().ToString(),
//                Email = Guid.NewGuid().ToString() + "@gmail.com"
//            };

//            var publisher = new Mock<IDomainEventOutboxPublisher>();
//            var customerRepository = new CustomerRepository(databaseContext);
//            var service = new CustomerService(customerRepository, publisher.Object, NullLogger<CustomerService>.Instance);

//            // Act
//            await service.CreateCustomerAsync(dto).ConfigureAwait(false);
//            await databaseContext.SaveChangesAsync().ConfigureAwait(false);

//            // Assert
//            Assert.True(databaseContext.Customers.Any(x => x.FirstName == dto.FirstName));
//        }
//    }
//}
