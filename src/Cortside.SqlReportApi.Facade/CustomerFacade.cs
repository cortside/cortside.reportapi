//using System;
//using System.Threading.Tasks;
//using Cortside.AspNetCore.Common.Paging;
//using Cortside.AspNetCore.EntityFramework;
//using Cortside.SqlReportApi.Facade.Mappers;

//namespace Cortside.SqlReportApi.Facade {

//    public class CustomerFacade : ICustomerFacade {
//        private readonly IUnitOfWork uow;
//        private readonly ICustomerService customerService;
//        private readonly CustomerMapper mapper;

//        public CustomerFacade(IUnitOfWork uow, ICustomerService customerService, CustomerMapper mapper) {
//            this.uow = uow;
//            this.customerService = customerService;
//            this.mapper = mapper;
//        }

//        public async Task<CustomerDto> CreateCustomerAsync(CustomerDto dto) {
//            var customer = await customerService.CreateCustomerAsync(dto);
//            await uow.SaveChangesAsync();

//            return mapper.MapToDto(customer);
//        }

//        public async Task<CustomerDto> GetCustomerAsync(Guid customerResourceId) {
//            // Using BeginNoTracking on GET endpoints for a single entity so that data is read committed
//            // with assumption that it might be used for changes in future calls
//            using (var tx = uow.BeginNoTracking()) {
//                var customer = await customerService.GetCustomerAsync(customerResourceId);
//                return mapper.MapToDto(customer);
//            }
//        }

//        public async Task PublishCustomerStateChangedEventAsync(Guid resourceId) {
//            await customerService.PublishCustomerStateChangedEventAsync(resourceId);
//            await uow.SaveChangesAsync();
//        }

//        public async Task<PagedList<CustomerDto>> SearchCustomersAsync(int pageSize, int pageNumber, string sortParams, CustomerSearch search) {
//            // Using BeginReadUncommittedAsync on GET endpoints that return a list, this will read uncommitted and
//            // as notracking in ef core.  this will result in a non-blocking dirty read, which is accepted best practice for mssql
//            using (var tx = await uow.BeginReadUncommitedAsync()) {
//                var customers = await customerService.SearchCustomersAsync(pageSize, pageNumber, sortParams, search);

//                return new PagedList<CustomerDto> {
//                    PageNumber = customers.PageNumber,
//                    PageSize = customers.PageSize,
//                    TotalItems = customers.TotalItems,
//                    Items = customers.Items.ConvertAll(x => mapper.MapToDto(x))
//                };
//            }
//        }

//        public async Task<CustomerDto> UpdateCustomerAsync(CustomerDto dto) {
//            var customer = await customerService.UpdateCustomerAsync(dto);
//            await uow.SaveChangesAsync();

//            return mapper.MapToDto(customer);
//        }
//    }
//}
