using System.Threading.Tasks;
using Cortside.SqlReportApi.Data;
using Cortside.SqlReportApi.DomainService;
using Microsoft.AspNetCore.Mvc;

namespace Cortside.SqlReportApi.WebApi.Controllers {

    [Route("api/reports")]
    public class ReportController : BaseController {

        public ReportController(IDatabaseContext db, SqlReportApiService svc) : base(db, svc) {
        }

        [HttpGet]
        public IActionResult Get() {
            var result = svc.GetReports();
            if (result == null) {
                return NotFound();
            }
            return new ObjectResult(result);
        }

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
