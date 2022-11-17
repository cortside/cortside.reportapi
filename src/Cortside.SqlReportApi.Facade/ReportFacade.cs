using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cortside.SqlReportApi.Data;
using Cortside.SqlReportApi.Domain.Entities;
using Cortside.SqlReportApi.DomainService;
using Cortside.SqlReportApi.Exceptions;
using Cortside.SqlReportApi.Facade.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cortside.SqlReportApi.Facade {

    public class ReportFacade {
        private readonly IUnitOfWork uow;
        private readonly ISqlReportService service;
        private readonly ReportMapper mapper;

        public ReportFacade(IUnitOfWork uow, ISqlReportService service, ReportMapper mapper) {
            this.uow = uow;
            this.service = service;
            this.mapper = mapper;
        }

        public async Task<IList<Report>> GetReportsAsync() {
            using (var tx = uow.BeginNoTracking()) {
                var result = await service.GetReportsAsync().ConfigureAwait(false);
                var results = new ListResult<Report>(result);
                return new ObjectResult(results);

                return mapper.MapToDto(reports);
            }
        }

        public Task<ReportResult> ExecuteReportAsync(string name, IQueryCollection query, List<string> list) {
            //    using (var tx = uow.BeginNoTracking()) {

            var authProperties = await policyClient.EvaluateAsync(User).ConfigureAwait(false);
            AuthorizationModel responseModel = new AuthorizationModel() {
                Permissions = authProperties.Permissions.ToList()
            };
            var permissionsPrefix = "Sql Report";
            responseModel.Permissions = responseModel.Permissions.Select(p => $"{permissionsPrefix}.{p}").ToList();
            try {
                var result = await facade.ExecuteReportAsync(name, Request.Query, authProperties.Permissions.ToList()).ConfigureAwait(false);
                return new ObjectResult(result);
            } catch (ResourceNotFoundMessage) {
                return new NotFoundResult();
            } catch (NotAuthorizedMessage) {
                return new UnauthorizedResult();
            }

            return mapper.MapToDto(result);
        }

        public Stream ExportReport(object report) {
            //    using (var tx = uow.BeginNoTracking()) {

            var authProperties = await policyClient.EvaluateAsync(User).ConfigureAwait(false);
            AuthorizationModel responseModel = new AuthorizationModel() {
                Permissions = authProperties.Permissions.ToList()
            };
            var permissionsPrefix = "Sql Report";
            responseModel.Permissions = responseModel.Permissions.ConvertAll(p => $"{permissionsPrefix}.{p}");
            try {
                var report = await facade.ExecuteReportAsync(name, Request.Query, authProperties.Permissions.ToList()).ConfigureAwait(false);
                Stream result = facade.ExportReport(report);
                return File(result, "application/octet-stream");
            } catch (ResourceNotFoundMessage) {
                return new NotFoundResult();
            } catch (NotAuthorizedMessage) {
                return new UnauthorizedResult();
            }

            return mapper.MapToDto(report);
        }

        //public async Task<CustomerDto> CreateCustomerAsync(CustomerDto dto) {
        //    var customer = await customerService.CreateCustomerAsync(dto).ConfigureAwait(false);
        //    await uow.SaveChangesAsync().ConfigureAwait(false);

        //    return mapper.MapToDto(customer);
        //}

        //public async Task<CustomerDto> GetCustomerAsync(Guid customerResourceId) {
        //    // Using BeginNoTracking on GET endpoints for a single entity so that data is read committed
        //    // with assumption that it might be used for changes in future calls
        //    using (var tx = uow.BeginNoTracking()) {
        //        var customer = await customerService.GetCustomerAsync(customerResourceId);
        //        return mapper.MapToDto(customer);
        //    }
        //}

        //public async Task PublishCustomerStateChangedEventAsync(Guid resourceId) {
        //    await customerService.PublishCustomerStateChangedEventAsync(resourceId).ConfigureAwait(false);
        //    await uow.SaveChangesAsync().ConfigureAwait(false);
        //}

        //public async Task<PagedList<CustomerDto>> SearchCustomersAsync(int pageSize, int pageNumber, string sortParams, CustomerSearch search) {
        //    // Using BeginReadUncommittedAsync on GET endpoints that return a list, this will read uncommitted and
        //    // as notracking in ef core.  this will result in a non-blocking dirty read, which is accepted best practice for mssql
        //    using (var tx = await uow.BeginReadUncommitedAsync().ConfigureAwait(false)) {
        //        var customers = await customerService.SearchCustomersAsync(pageSize, pageNumber, sortParams, search).ConfigureAwait(false);

        //        return new PagedList<CustomerDto> {
        //            PageNumber = customers.PageNumber,
        //            PageSize = customers.PageSize,
        //            TotalItems = customers.TotalItems,
        //            Items = customers.Items.ConvertAll(x => mapper.MapToDto(x))
        //        };
        //    }
        //}

        //public async Task<CustomerDto> UpdateCustomerAsync(CustomerDto dto) {
        //    var customer = await customerService.UpdateCustomerAsync(dto).ConfigureAwait(false);
        //    await uow.SaveChangesAsync().ConfigureAwait(false);

        //    return mapper.MapToDto(customer);
        //}
    }
}
