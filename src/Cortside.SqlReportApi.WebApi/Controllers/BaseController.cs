using Cortside.SqlReportApi.Data;
using Cortside.SqlReportApi.DomainService;
using Microsoft.AspNetCore.Mvc;

namespace Cortside.SqlReportApi.WebApi.Controllers {

    /// <summary>
    /// Shared functionality/resource for the api
    /// </summary>
    [ApiVersion("1")]
    [Produces("application/json")]
    [ApiController]
    public abstract class BaseController : Controller {
        /// <summary>
        /// Base route
        /// </summary>
        protected const string BaseRoute = "api/v1/";
        /// <summary>
        /// Database
        /// </summary>
        protected IDatabaseContext db;
        /// <summary>
        /// Report Service
        /// </summary>
        protected ISqlReportService svc;

        /// <summary>
        /// Initialize the base controller
        /// </summary>
        /// <param name="db"></param>
        /// <param name="svc"></param>
        public BaseController(IDatabaseContext db, ISqlReportService svc) {
            this.db = db;
            this.svc = svc;
        }
    }
}
