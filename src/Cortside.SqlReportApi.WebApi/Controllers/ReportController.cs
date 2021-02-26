using System.Threading.Tasks;
using Cortside.SqlReportApi.Data;
using Cortside.SqlReportApi.DomainService;
using Microsoft.AspNetCore.Mvc;

namespace Cortside.SqlReportApi.WebApi.Controllers {

    /// <summary>
    /// Access functionality for reports
    /// </summary>
    [Route(BaseRoute + "reports")]
    public class ReportController : BaseController {

        /// <summary>
        /// Initialize the base controller
        /// </summary>
        /// <param name="db"></param>
        /// <param name="svc"></param>
        public ReportController(IDatabaseContext db, ISqlReportService svc) : base(db, svc) {
        }

        /// <summary>
        /// Get all reports
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get() {
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
            var result = await svc.ExecuteReport(name, Request.Query);
            if (result == null) {
                return NotFound();
            }
            return new ObjectResult(result);
        }
    }
}
