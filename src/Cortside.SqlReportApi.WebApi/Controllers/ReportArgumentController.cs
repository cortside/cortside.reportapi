using Cortside.SqlReportApi.Data;
using Cortside.SqlReportApi.DomainService;
using Microsoft.AspNetCore.Mvc;
using PolicyServer.Runtime.Client;

namespace Cortside.SqlReportApi.WebApi.Controllers {

    /// <summary>
    /// Access functionality for report arguments
    /// </summary>
    [Route(BaseRoute + "arguments")]
    public class ReportArgumentController : BaseController {

        /// <summary>
        /// Initialize the controller
        /// </summary>
        /// <param name="db"></param>
        /// <param name="svc"></param>
        public ReportArgumentController(IDatabaseContext db, ISqlReportService svc, IPolicyServerRuntimeClient policyClient) : base(db, svc, policyClient) {
        }

        /// <summary>
        /// Get all report arguments
        /// </summary>
        /// <returns></returns>
        [HttpGet]
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
        public IActionResult Get(int id) {
            var result = svc.GetReportArgument(id);
            if (result == null) {
                return NotFound();
            }
            return new ObjectResult(result);
        }
    }
}
