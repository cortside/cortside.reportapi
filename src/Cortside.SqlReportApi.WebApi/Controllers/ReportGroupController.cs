using Cortside.SqlReportApi.Data;
using Cortside.SqlReportApi.DomainService;
using Microsoft.AspNetCore.Mvc;

namespace Cortside.SqlReportApi.WebApi.Controllers {

    /// <summary>
    /// Access functionality for report groups
    /// </summary>
    [Route(BaseRoute + "reportgroups")]
    public class ReportGroupController : BaseController {

        /// <summary>
        /// Initialize the controller
        /// </summary>
        /// <param name="db"></param>
        /// <param name="svc"></param>
        public ReportGroupController(IDatabaseContext db, SqlReportService svc) : base(db, svc) {
        }

        /// <summary>
        /// Get all report groups
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get() {
            var result = svc.GetReportGroups();
            if (result == null) {
                return NotFound();
            }
            return new ObjectResult(result);
        }

        /// <summary>
        /// Get a report group by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id) {
            var result = svc.GetReportGroup(id);
            if (result == null) {
                return NotFound();
            }
            return new ObjectResult(result);
        }
    }
}
