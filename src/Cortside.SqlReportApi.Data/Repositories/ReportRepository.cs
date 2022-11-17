using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Cortside.SqlReportApi.Domain.Entities;
using Cortside.SqlReportApi.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Cortside.SqlReportApi.Data.Repositories {
    public class ReportRepository : IReportRepository {
        private readonly IDatabaseContext context;

        public ReportRepository(IDatabaseContext context) {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Report> GetReportAsync(string name) {
            var report = await context.Reports
                .Include(r => r.ReportArguments)
                .ThenInclude(y => y.ReportArgumentQuery)
                .Include(g => g.ReportGroup)
                .Where(p => p.Name.Equals(name)).SingleOrDefaultAsync().ConfigureAwait(false);
            if (report != null) {
                foreach (var arg in report.ReportArguments) {
                    arg.ArgValues = GetArgumentPairs(arg);
                }
            } else {
                throw new ResourceNotFoundMessage($"Report {name} could not be found.");
            }
            return report;
        }

        public async Task<IList<Report>> GetReportsAsync() {
            var reports = await context.Reports
                .Include(r => r.ReportArguments)
                .ThenInclude(y => y.ReportArgumentQuery)
                .Include(g => g.ReportGroup)
                .ToListAsync().ConfigureAwait(false);
            foreach (var report in reports) {
                foreach (var arg in report.ReportArguments) {
                    arg.ArgValues = GetArgumentPairs(arg);
                }
            }
            return reports;
        }

        public async Task<IList<ReportGroup>> GetReportGroupsAsync() {
            return await context.ReportGroups.ToListAsync().ConfigureAwait(false);
        }

        public async Task<ReportGroup> GetReportGroupAsync(int id) {
            var reportGroup = await context.ReportGroups
                .Where(p => p.ReportGroupId.Equals(id))
                .SingleOrDefaultAsync().ConfigureAwait(false);
            if (reportGroup == null) {
                throw new ResourceNotFoundMessage($"ReportGroup {id} could not be found.");
            }
            return reportGroup;
        }

        public async Task<IList<ReportArgument>> GetReportArgumentsAsync() {
            var args = await context.ReportArguments
                .Include(y => y.ReportArgumentQuery)
                .ToListAsync().ConfigureAwait(false);
            foreach (var arg in args) {
                arg.ArgValues = GetArgumentPairs(arg);
            }

            return args;
        }

        private Dictionary<string, object> GetArgumentPairs(ReportArgument arg) {
            var pairs = new Dictionary<string, object>();
            if (arg?.ReportArgumentQueryId.HasValue == true) {
                using (var cmd = context.Database.GetDbConnection().CreateCommand()) {
                    cmd.CommandText = arg.ReportArgumentQuery.ArgQuery;
                    context.Database.GetDbConnection().Open();
                    try {
                        using (var reader = cmd.ExecuteReader()) {
                            while (reader.Read()) {
                                var key = reader.GetString(0);
                                var value = reader.GetValue(1);
                                pairs.Add(key, value);
                            }
                        }
                    } finally {
                        context.Database.GetDbConnection().Close();
                    }
                }
            }
            return pairs;
        }

        public async Task<ReportArgument> GetReportArgumentAsync(int id) {
            var arg = await context.ReportArguments.Include(y => y.ReportArgumentQuery).Where(p => p.ReportArgumentId.Equals(id)).SingleOrDefaultAsync().ConfigureAwait(false);
            if (arg != null) {
                arg.ArgValues = GetArgumentPairs(arg);
            }
            return arg;
        }

        public async Task<IList<ReportArgumentQuery>> GetReportArgumentQueriesAsync() {
            return await context.ReportArgumentQuerys.ToListAsync().ConfigureAwait(false);
        }

        public Task<ReportArgumentQuery> GetReportArgumentQueryAsync(int id) {
            return context.ReportArgumentQuerys.Where(p => p.ReportArgumentQueryId.Equals(id)).SingleOrDefaultAsync();
        }

        public async Task<ReportResult> ExecuteReportAsync(string name, IQueryCollection args, List<string> permissions) {
            var report = await GetReportAsync(name);
            if (report == null) {
                throw new ResourceNotFoundMessage($"Report {name} could not be found.");
            }

            if (!permissions.Contains(report.Permission)) {
                throw new NotAuthorizedMessage($"The requested resource requires the permission: {report.Permission}.");
            }

            ReportResult result = new ReportResult(name);
            await using (var cmd = context.Database.GetDbConnection().CreateCommand()) {
                cmd.CommandText = name;
                cmd.CommandType = CommandType.StoredProcedure;

                await context.Database.GetDbConnection().OpenAsync().ConfigureAwait(false);

                try {
                    foreach (var arg in report.ReportArguments) {
                        var p = cmd.CreateParameter();
                        p.ParameterName = arg.ArgName;

                        if (args.ContainsKey(arg.ArgName.Replace("@", ""))) {
                            var argValue = args[arg.ArgName.Replace("@", "")].FirstOrDefault();
                            p.Value = argValue;
                        } else {
                            p.Value = DBNull.Value;
                        }
                        cmd.Parameters.Add(p);
                    }

                    await using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false)) {
                        do {
                            var resultset = new ResultSet();
                            result.ResultSets.Add(resultset);

                            var schema = await reader.GetSchemaTableAsync().ConfigureAwait(false);
                            foreach (DataRow row in schema.Rows) {
                                var column = new ReportColumn {
                                    Name = row[schema.Columns.IndexOf("ColumnName")].ToString(),
                                    DataType = row[schema.Columns.IndexOf("DataType")].ToString(),
                                    Ordinal = (int)row[schema.Columns.IndexOf("ColumnOrdinal")]
                                };
                                resultset.Columns.Add(column);
                            }

                            while (await reader.ReadAsync().ConfigureAwait(false)) {
                                var row = new object[reader.FieldCount];
                                reader.GetValues(row);
                                resultset.Rows.Add(row);
                            }
                        } while (await reader.NextResultAsync().ConfigureAwait(false));
                    }
                } finally {
                    await context.Database.GetDbConnection().CloseAsync().ConfigureAwait(false);
                }
            }
            return result;
        }




























        //public async Task<PagedList<Customer>> SearchAsync(int pageSize, int pageNumber, string sortParams, CustomerSearch model) {
        //    var customers = model.Build(context.Customers.Include(x => x.CreatedSubject).Include(x => x.LastModifiedSubject).AsNoTracking());
        //    var result = new PagedList<Customer> {
        //        PageNumber = pageNumber,
        //        PageSize = pageSize,
        //        TotalItems = await customers.CountAsync().ConfigureAwait(false),
        //        Items = new List<Customer>(),
        //    };

        //    customers = customers.ToSortedQuery(sortParams);
        //    result.Items = await customers.ToPagedQuery(pageNumber, pageSize).ToListAsync().ConfigureAwait(false);

        //    return result;
        //}

        //public Customer Add(Customer customer) {
        //    var entity = context.Customers.Add(customer);
        //    return entity.Entity;
        //}

        //public Task<Customer> GetAsync(Guid id) {
        //    return context.Customers
        //        .Include(x => x.CreatedSubject)
        //        .Include(x => x.LastModifiedSubject)
        //        .FirstOrDefaultAsync(o => o.CustomerResourceId == id);
        //}
    }
}
