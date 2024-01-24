//using System;
//using System.Threading.Tasks;
//using Cortside.SqlReportApi.Data.Searches;
//using Cortside.SqlReportApi.Domain.Entities;
//using Cortside.AspNetCore.Common.Paging;

//namespace Cortside.SqlReportApi.Data.Repositories {
//    public interface ICustomerRepository : IRepository<Customer> {
//        Customer Add(Customer customer);
//        Task<Customer> GetAsync(Guid id);
//        Task<PagedList<Customer>> SearchAsync(int pageSize, int pageNumber, string sortParams, CustomerSearch model);
//    }
//}
