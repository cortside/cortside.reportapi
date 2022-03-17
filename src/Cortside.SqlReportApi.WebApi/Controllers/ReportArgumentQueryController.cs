using System.Threading.Tasks;
using Cortside.SqlReportApi.Data;
using Cortside.SqlReportApi.DomainService;
using Microsoft.AspNetCore.Mvc;
using PolicyServer.Runtime.Client;

namespace Cortside.SqlReportApi.WebApi.Controllers {

    /// <summary>
    /// Access functionality for report argument queries
    /// </summary>
    [Route(BaseRoute + "argumentqueries")]
    public class ReportArgumentQueryController : BaseController {
        private readonly IPolicyServerRuntimeClient policyClient;
        /// <summary>
        /// Initialize the controller
        /// </summary>
        /// <param name="db"></param>
        /// <param name="svc"></param>
        public ReportArgumentQueryController(DatabaseContext db, ISqlReportService svc, IPolicyServerRuntimeClient policyClient) : base(db, svc, policyClient) {
            this.policyClient = policyClient;
        }

        /// <summary>
        /// Get all queries
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Authorize(Constants.Authorization.Permissions.CanGetReports)]
        public async Task<IActionResult> Get() {

            var result = svc.GetReportArgumentQueries();
            if (result == null) {
                return NotFound();
            }
            return new ObjectResult(result);
        }

        /// <summary>
        /// Get a query by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        //[Authorize(Constants.Authorization.Permissions.CanGetReports)]
        public IActionResult Get(int id) {
            var result = svc.GetReportArgumentQuery(id);
            if (result == null) {
                return NotFound();
            }
            return new ObjectResult(result);
        }
    }
}
