using Cortside.SqlReportApi.Data;
using Cortside.SqlReportApi.DomainService;
using Microsoft.AspNetCore.Mvc;

namespace Cortside.SqlReportApi.WebApi.Controllers {

    public abstract class BaseController : Controller {
        protected IDatabaseContext db;
        protected SqlReportApiService svc;

        public BaseController(IDatabaseContext db, SqlReportApiService svc) {
            this.db = db;
            this.svc = svc;
        }
    }
}
