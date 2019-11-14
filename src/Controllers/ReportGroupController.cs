using Microsoft.AspNetCore.Mvc;
using webapi.Reporting.Data;
using webapi.Reporting.Service;

namespace webapi.Controllers {
    [Route("api/reportgroups")]
    public class ReportGroupController : BaseController {

        public ReportGroupController(MyContext ctx, ReportService svc) : base(ctx, svc) {
        }

        [HttpGet]
        public IActionResult Get() {
            var result = svc.GetReportGroups();
            if (result == null) {
                return NotFound();
            }
            return new ObjectResult(result);
        }

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
