using Cortside.SqlReportApi.Data;
using Cortside.SqlReportApi.DomainService;
using Microsoft.AspNetCore.Mvc;

namespace Cortside.SqlReportApi.WebApi.Controllers {

    [Route("api/arguments")]
    public class ReportArgumentController : BaseController {

        public ReportArgumentController(IDatabaseContext db, SqlReportApiService svc) : base(db, svc) {
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
