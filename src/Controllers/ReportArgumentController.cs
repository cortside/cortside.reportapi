using Microsoft.AspNetCore.Mvc;
using webapi.Reporting.Data;
using webapi.Reporting.Service;

namespace webapi.Controllers {
    [Route("api/arguments")]
    public class ReportArgumentController : BaseController {

        public ReportArgumentController(MyContext ctx, ReportService svc) : base(ctx, svc) {
        }

        [HttpGet]
        public IActionResult Get() {
            var result = svc.GetReportArguments();
            if (result == null) {
                return NotFound();
            }
            return new ObjectResult(result);
        }

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
