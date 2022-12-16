//using System;
//using System.Threading.Tasks;
//using Cortside.AspNetCore.Common.Paging;

//namespace Cortside.SqlReportApi.Facade {

//    public interface ICustomerFacade {

//        Task<CustomerDto> CreateCustomerAsync(CustomerDto dto);

//        Task<CustomerDto> GetCustomerAsync(Guid customerResourceId);

//        Task<PagedList<CustomerDto>> SearchCustomersAsync(int pageSize, int pageNumber, string sortParams, CustomerSearch search);

//        Task<CustomerDto> UpdateCustomerAsync(CustomerDto dto);

//        Task PublishCustomerStateChangedEventAsync(Guid resourceId);
//    }
//}
