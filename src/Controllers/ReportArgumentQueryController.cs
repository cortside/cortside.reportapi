using Microsoft.AspNetCore.Mvc;
using webapi.Reporting.Data;
using webapi.Reporting.Service;

namespace webapi.Controllers {
    [Route("api/argumentqueries")]
    public class ReportArgumentQueryController : BaseController {

        public ReportArgumentQueryController(MyContext ctx, ReportService svc) : base(ctx, svc) {
        }

        [HttpGet]
        public IActionResult Get() {
            var result = svc.GetReportArgumentQueries();
            if (result == null) {
                return NotFound();
            }
            return new ObjectResult(result);
        }

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
