using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webapi.Reporting.Data;
using webapi.Reporting.Service;

namespace webapi.Controllers {
    [Route("api/reports")]
    public class ReportController : BaseController {
        public ReportController(MyContext ctx, ReportService svc) : base(ctx, svc) {
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
