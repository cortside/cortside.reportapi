using Cortside.SqlReportApi.Data;
using Cortside.SqlReportApi.DomainService;
using Microsoft.AspNetCore.Mvc;
using PolicyServer.Runtime.Client;

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
        protected DatabaseContext db;
        /// <summary>
        /// Report Service
        /// </summary>
        protected ISqlReportService svc;

        /// <summary>
        /// Policy client to check permissions
        /// </summary>
        protected readonly IPolicyServerRuntimeClient policyClient;

        /// <summary>
        /// Initialize the base controller
        /// </summary>
        /// <param name="db"></param>
        /// <param name="svc"></param>
        public BaseController(DatabaseContext db, ISqlReportService svc, IPolicyServerRuntimeClient policyClient) {
            this.db = db;
            this.svc = svc;
            this.policyClient = policyClient;
        }
    }
}
