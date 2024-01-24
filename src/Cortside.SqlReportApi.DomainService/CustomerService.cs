//using System;
//using System.Threading.Tasks;
//using Cortside.DomainEvent.Events;
//using Cortside.SqlReportApi.Data.Repositories;
//using Cortside.SqlReportApi.Data.Searches;
//using Cortside.SqlReportApi.Domain.Entities;
//using Cortside.SqlReportApi.Dto;
//using Cortside.AspNetCore.Common.Paging;
//using Cortside.DomainEvent;
//using Microsoft.Extensions.Logging;
//using Serilog.Context;

//namespace Cortside.SqlReportApi.DomainService {
//    public class CustomerService : ICustomerService {
//        private readonly IDomainEventOutboxPublisher publisher;
//        private readonly ILogger<CustomerService> logger;
//        private readonly ICustomerRepository customerRepository;

//        public CustomerService(ICustomerRepository customerRepository, IDomainEventOutboxPublisher publisher, ILogger<CustomerService> logger) {
//            this.publisher = publisher;
//            this.logger = logger;
//            this.customerRepository = customerRepository;
//        }

//        public async Task<Customer> CreateCustomerAsync(CustomerDto dto) {
//            var entity = new Customer(dto.FirstName, dto.LastName, dto.Email);
//            using (LogContext.PushProperty("CustomerResourceId", entity.CustomerResourceId)) {
//                customerRepository.Add(entity);
//                logger.LogInformation("Created new customer");

//                var @event = new CustomerStateChangedEvent() { CustomerResourceId = entity.CustomerResourceId, Timestamp = entity.LastModifiedDate };
//                await publisher.PublishAsync(@event).ConfigureAwait(false);

//                return entity;
//            }
//        }

//        public async Task<Customer> GetCustomerAsync(Guid customerResourceId) {
//            var entity = await customerRepository.GetAsync(customerResourceId).ConfigureAwait(false);
//            return entity;
//        }

//        public Task<PagedList<Customer>> SearchCustomersAsync(int pageSize, int pageNumber, string sortParams, CustomerSearch search) {
//            return customerRepository.SearchAsync(pageSize, pageNumber, sortParams, search);
//        }

//        public async Task<Customer> UpdateCustomerAsync(CustomerDto dto) {
//            var entity = await customerRepository.GetAsync(dto.CustomerResourceId).ConfigureAwait(false);
//            using (LogContext.PushProperty("CustomerResourceId", entity.CustomerResourceId)) {
//                entity.Update(dto.FirstName, dto.LastName, dto.Email);
//                logger.LogInformation("Updated existing customer");

//                var @event = new CustomerStateChangedEvent() { CustomerResourceId = entity.CustomerResourceId, Timestamp = entity.LastModifiedDate };
//                await publisher.PublishAsync(@event).ConfigureAwait(false);

//                return entity;
//            }
//        }

//        public async Task PublishCustomerStateChangedEventAsync(Guid resourceId) {
//            var entity = await customerRepository.GetAsync(resourceId).ConfigureAwait(false);

//            var @event = new CustomerStateChangedEvent() { CustomerResourceId = entity.CustomerResourceId, Timestamp = entity.LastModifiedDate };
//            await publisher.PublishAsync(@event).ConfigureAwait(false);
//        }
//    }
//}
