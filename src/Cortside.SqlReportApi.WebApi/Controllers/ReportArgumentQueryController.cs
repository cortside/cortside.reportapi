using Cortside.SqlReportApi.Data;
using Cortside.SqlReportApi.DomainService;
using Microsoft.AspNetCore.Mvc;

namespace Cortside.SqlReportApi.WebApi.Controllers {

    /// <summary>
    /// Access functionality for report argument queries
    /// </summary>
    [Route(BaseRoute + "argumentqueries")]
    public class ReportArgumentQueryController : BaseController {

        /// <summary>
        /// Initialize the controller
        /// </summary>
        /// <param name="db"></param>
        /// <param name="svc"></param>
        public ReportArgumentQueryController(IDatabaseContext db, SqlReportService svc) : base(db, svc) {
        }

        /// <summary>
        /// Get all queries
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get() {
            var result = svc.GetReportArgumentQueries();
            if (result == null) {
                return NotFound();
            }
            return new ObjectResult(result);
        }

        /// <summary>
        /// Get a query by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id) {
            var result = svc.GetReportArgumentQuery(id);
            if (result == null) {
                return NotFound();
            }
            return new ObjectResult(result);
        }
    }
}
