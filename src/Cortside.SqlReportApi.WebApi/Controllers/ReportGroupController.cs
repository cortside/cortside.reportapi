using Cortside.SqlReportApi.Data;
using Cortside.SqlReportApi.DomainService;
using Microsoft.AspNetCore.Mvc;

namespace Cortside.SqlReportApi.WebApi.Controllers {

    [Route("api/reportgroups")]
    public class ReportGroupController : BaseController {

        public ReportGroupController(IDatabaseContext db, SqlReportApiService svc) : base(db, svc) {
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
