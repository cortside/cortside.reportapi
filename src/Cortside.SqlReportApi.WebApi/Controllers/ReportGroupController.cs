using System.Threading.Tasks;
using Cortside.AspNetCore.Common.Models;
using Cortside.SqlReportApi.Domain.Entities;
using Cortside.SqlReportApi.DomainService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cortside.SqlReportApi.WebApi.Controllers {
    /// <summary>
    /// Access functionality for report groups
    /// </summary>
    [Route("api/v{version:apiVersion}/reportgroups")]
    [ApiVersion("1")]
    [Produces("application/json")]
    [ApiController]
    public class ReportGroupController : Controller {
        private readonly ISqlReportService svc;

        /// <summary>
        /// Initialize the controller
        /// </summary>
        /// <param name="svc"></param>
        public ReportGroupController(ISqlReportService svc) {
            this.svc = svc;
        }

        /// <summary>
        /// Get all report groups
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        [ProducesResponseType(typeof(ListResult<ReportGroup>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync() {
            var groups = await svc.GetReportGroupsAsync().ConfigureAwait(false);
            var result = new ListResult<ReportGroup>(groups);
            return Ok(result);
        }

        /// <summary>
        /// Get a report group by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReportGroup), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync(int id) {
            var result = await svc.GetReportGroupAsync(id).ConfigureAwait(false);
            if (result == null) {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
