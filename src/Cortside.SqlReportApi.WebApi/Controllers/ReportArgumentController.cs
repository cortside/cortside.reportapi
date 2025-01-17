using System.Threading.Tasks;
using Asp.Versioning;
using Cortside.AspNetCore.Common.Models;
using Cortside.SqlReportApi.Domain.Entities;
using Cortside.SqlReportApi.DomainService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cortside.SqlReportApi.WebApi.Controllers {
    /// <summary>
    /// Access functionality for report arguments
    /// </summary>
    [Route("api/v{version:apiVersion}/arguments")]
    [ApiVersion("1")]
    [Produces("application/json")]
    [ApiController]
    public class ReportArgumentController : ControllerBase {
        private readonly ISqlReportService svc;

        /// <summary>
        /// Initialize the controller
        /// </summary>
        /// <param name="svc"></param>
        public ReportArgumentController(ISqlReportService svc) {
            this.svc = svc;
        }

        /// <summary>
        /// Get all report arguments
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ListResult<ReportArgument>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync() {
            var result = await svc.GetReportArgumentsAsync().ConfigureAwait(false);
            if (result == null) {
                return NotFound();
            }
            var results = new ListResult<ReportArgument>(result);
            return new ObjectResult(results);
        }

        /// <summary>
        /// Get report argument by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReportArgument), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync(int id) {
            var result = await svc.GetReportArgumentAsync(id).ConfigureAwait(false);
            if (result == null) {
                return NotFound();
            }
            return new ObjectResult(result);
        }
    }
}
