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
    public class ReportArgumentController : Controller {
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
        [ProducesResponseType(typeof(ListResult<ReportGroup>), StatusCodes.Status200OK)]
        public IActionResult Get() {
            var result = svc.GetReportArguments();
            if (result == null) {
                return NotFound();
            }
            return new ObjectResult(result);
        }

        /// <summary>
        /// Get report argument by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ListResult<ReportGroup>), StatusCodes.Status200OK)]
        public IActionResult Get(int id) {
            var result = svc.GetReportArgument(id);
            if (result == null) {
                return NotFound();
            }
            return new ObjectResult(result);
        }
    }
}
