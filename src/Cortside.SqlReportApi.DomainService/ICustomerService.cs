//using System;
//using System.Threading.Tasks;
//using Cortside.SqlReportApi.Data.Searches;
//using Cortside.SqlReportApi.Domain.Entities;
//using Cortside.SqlReportApi.Dto;
//using Cortside.AspNetCore.Common.Paging;

//namespace Cortside.SqlReportApi.DomainService {
//    public interface ICustomerService {
//        Task<Customer> CreateCustomerAsync(CustomerDto dto);
//        Task<Customer> GetCustomerAsync(Guid customerResourceId);
//        Task<PagedList<Customer>> SearchCustomersAsync(int pageSize, int pageNumber, string sortParams, CustomerSearch search);
//        Task<Customer> UpdateCustomerAsync(CustomerDto dto);
//        Task PublishCustomerStateChangedEventAsync(Guid resourceId);
//    }
//}
