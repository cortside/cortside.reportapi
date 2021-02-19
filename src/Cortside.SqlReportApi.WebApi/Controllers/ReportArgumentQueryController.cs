using Cortside.SqlReportApi.Data;
using Cortside.SqlReportApi.DomainService;
using Microsoft.AspNetCore.Mvc;

namespace Cortside.SqlReportApi.WebApi.Controllers {

    [Route("api/argumentqueries")]
    public class ReportArgumentQueryController : BaseController {

        public ReportArgumentQueryController(IDatabaseContext db, SqlReportApiService svc) : base(db, svc) {
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
