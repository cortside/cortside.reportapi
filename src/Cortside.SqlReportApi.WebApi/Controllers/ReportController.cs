using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Cortside.SqlReportApi.Data;
using Cortside.SqlReportApi.DomainService;
using Cortside.SqlReportApi.Exceptions;
using Cortside.SqlReportApi.WebApi.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using PolicyServer.Runtime.Client;

namespace Cortside.SqlReportApi.WebApi.Controllers {

    /// <summary>
    /// Access functionality for reports
    /// </summary>
    [Route(BaseRoute + "reports")]
    public class ReportController : BaseController {

        /// <summary>
        /// Initialize the base controller        /// </summary>
        /// <param name="db"></param>
        /// <param name="svc"></param>
        /// <param name="policyClient"></param>
        public ReportController(DatabaseContext db, ISqlReportService svc, IPolicyServerRuntimeClient policyClient) : base(db, svc, policyClient) {
        }

        /// <summary>
        /// Get all reports
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Authorize(Constants.Authorization.Permissions.CanGetReports)]
        public async Task<IActionResult> Get() {
            var result = svc.GetReports();
            if (result == null) {
                return NotFound();
            }
            return new ObjectResult(result);
        }

        /// <summary>
        /// Get a report by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> Get(string name) {
            var authProperties = await policyClient.EvaluateAsync(User);
            AuthorizationModel responseModel = new AuthorizationModel() {
                Permissions = authProperties.Permissions.ToList()
            };
            var permissionsPrefix = "Sql Report";
            responseModel.Permissions = responseModel.Permissions.Select(p => $"{permissionsPrefix}.{p}").ToList();
            try {
                var result = await svc.ExecuteReport(name, Request.Query, authProperties.Permissions.ToList());
                return new ObjectResult(result);
            } catch (ResourceNotFoundMessage) {
                return new NotFoundResult();
            } catch (NotAuthorizedMessage) {
                return new UnauthorizedResult();
            }
        }

        /// <summary>
        /// Export report as csv
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{name}/export")]
        public async Task<IActionResult> Export(string name) {
            var authProperties = await policyClient.EvaluateAsync(User);
            AuthorizationModel responseModel = new AuthorizationModel() {
                Permissions = authProperties.Permissions.ToList()
            };
            var permissionsPrefix = "Sql Report";
            responseModel.Permissions = responseModel.Permissions.Select(p => $"{permissionsPrefix}.{p}").ToList();
            try {
                var report = await svc.ExecuteReport(name, Request.Query, authProperties.Permissions.ToList());
                Stream result = svc.ExportReport(report);
                var file = File(result, "application/octet-stream");
                return new ObjectResult(file);
            } catch (ResourceNotFoundMessage) {
                return new NotFoundResult();
            } catch (NotAuthorizedMessage) {
                return new UnauthorizedResult();
            }
        }
    }
}
