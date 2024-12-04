using System.Threading.Tasks;
using Asp.Versioning;
using Cortside.AspNetCore.Common.Models;
using Cortside.SqlReportApi.Domain.Entities;
using Cortside.SqlReportApi.DomainService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cortside.SqlReportApi.WebApi.Controllers {
    /// <summary>
    /// Access functionality for report argument queries
    /// </summary>
    [Route("api/v{version:apiVersion}/argumentqueries")]
    [ApiVersion("1")]
    [Produces("application/json")]
    [ApiController]
    public class ReportArgumentQueryController : Controller {
        private readonly ISqlReportService svc;

        /// <summary>
        /// Initialize the controller
        /// </summary>
        /// <param name="svc"></param>
        public ReportArgumentQueryController(ISqlReportService svc) {
            this.svc = svc;
        }

        /// <summary>
        /// Get all queries
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ListResult<ReportArgumentQuery>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync() {
            var result = await svc.GetReportArgumentQueriesAsync().ConfigureAwait(false);
            if (result == null) {
                return NotFound();
            }
            var results = new ListResult<ReportArgumentQuery>(result);
            return Ok(results);
        }

        /// <summary>
        /// Get a query by ID
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReportArgumentQuery), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync(int id) {
            var result = await svc.GetReportArgumentQueryAsync(id).ConfigureAwait(false);
            if (result == null) {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
