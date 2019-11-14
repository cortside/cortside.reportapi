using Microsoft.AspNetCore.Mvc;
using webapi.Reporting.Data;
using webapi.Reporting.Service;

namespace webapi.Controllers {
    public abstract class BaseController : Controller {
        protected MyContext ctx;
        protected ReportService svc;

        public BaseController(MyContext ctx, ReportService svc) {
            this.ctx = ctx;
            this.svc = svc;
        }
    }
}
