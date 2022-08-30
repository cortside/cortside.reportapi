using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cortside.AspNetCore.Common.Models;
using Cortside.SqlReportApi.Data;
using Cortside.SqlReportApi.Domain.Entities;
using Cortside.SqlReportApi.DomainService;
using Cortside.SqlReportApi.Exceptions;
using Cortside.SqlReportApi.WebApi.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PolicyServer.Runtime.Client;

namespace Cortside.SqlReportApi.WebApi.Controllers {
    /// <summary>
    /// Access functionality for reports
    /// </summary>
    [Route("api/v{version:apiVersion}/reports")]
    [ApiVersion("1")]
    [Produces("application/json")]
    [ApiController]
    public class ReportController : Controller {
        private readonly ISqlReportService svc;
        private readonly IPolicyServerRuntimeClient policyClient;

        /// <summary>
        /// Initialize the base controller
        /// </summary>
        /// <param name="db"></param>
        /// <param name="svc"></param>
        /// <param name="policyClient"></param>
        public ReportController(ISqlReportService svc, IPolicyServerRuntimeClient policyClient) {
            this.svc = svc;
            this.policyClient = policyClient;
        }

        /// <summary>
        /// Get all reports
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ListResult<ReportGroup>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync() {
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
        [HttpGet("{name}")]
        [ProducesResponseType(typeof(ReportResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync(string name) {
            var authProperties = await policyClient.EvaluateAsync(User).ConfigureAwait(false);
            AuthorizationModel responseModel = new AuthorizationModel() {
                Permissions = authProperties.Permissions.ToList()
            };
            var permissionsPrefix = "Sql Report";
            responseModel.Permissions = responseModel.Permissions.Select(p => $"{permissionsPrefix}.{p}").ToList();
            try {
                var result = await svc.ExecuteReportAsync(name, Request.Query, authProperties.Permissions.ToList()).ConfigureAwait(false);
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
        [HttpGet("{name}/export")]
        [ProducesResponseType(typeof(Stream), StatusCodes.Status200OK)]
        public async Task<IActionResult> ExportAsync(string name) {
            var authProperties = await policyClient.EvaluateAsync(User).ConfigureAwait(false);
            AuthorizationModel responseModel = new AuthorizationModel() {
                Permissions = authProperties.Permissions.ToList()
            };
            var permissionsPrefix = "Sql Report";
            responseModel.Permissions = responseModel.Permissions.ConvertAll(p => $"{permissionsPrefix}.{p}");
            try {
                var report = await svc.ExecuteReportAsync(name, Request.Query, authProperties.Permissions.ToList()).ConfigureAwait(false);
                Stream result = svc.ExportReport(report);
                return File(result, "application/octet-stream");
            } catch (ResourceNotFoundMessage) {
                return new NotFoundResult();
            } catch (NotAuthorizedMessage) {
                return new UnauthorizedResult();
            }
        }
    }
}
