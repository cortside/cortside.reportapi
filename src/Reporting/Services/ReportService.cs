using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using webapi.Reporting.Data;
using webapi.Reporting.Entity;

namespace webapi.Reporting.Service {
    public class ReportService {

        protected MyContext ctx;

        public ReportService(MyContext ctx) {
            this.ctx = ctx;
        }

        public Report GetReport(string name) {
            var report = ctx.Reports
                .Include(r => r.ReportArguments)
                .ThenInclude(y => y.ReportArgumentQuery)
                .Where(p => p.Name.Equals(name)).SingleOrDefault();
            if (report != null) {
                foreach (var arg in report.ReportArguments) {
                    arg.ArgValues = GetArgumentPairs(arg);
                }
            }
            return report;
        }

        public IEnumerable<Report> GetReports() {
            var reports = ctx.Reports
                .Include(r => r.ReportArguments)
                .ThenInclude(y => y.ReportArgumentQuery)
                .ToList();
            foreach (var report in reports) {
                foreach (var arg in report.ReportArguments) {
                    arg.ArgValues = GetArgumentPairs(arg);
                }
            }
            return reports;
        }

        public IEnumerable<ReportGroup> GetReportGroups() {
            return ctx.ReportGroups.ToList();
        }

        public ReportGroup GetReportGroup(int id) {
            return ctx.ReportGroups.Where(p => p.ReportGroupId.Equals(id)).SingleOrDefault();
        }

        public IEnumerable<ReportArgument> GetReportArguments() {
            var args = ctx.ReportArguments.Include(y => y.ReportArgumentQuery).ToList();
            foreach (var arg in args) {
                arg.ArgValues = GetArgumentPairs(arg);
            }

            return args;
        }

        protected Dictionary<object, object> GetArgumentPairs(ReportArgument arg) {
            if (arg != null && arg.ReportArgumentQueryId.HasValue) {
                var pairs = new Dictionary<object, object>();

                using (var cmd = ctx.Database.GetDbConnection().CreateCommand()) {
                    cmd.CommandText = arg.ReportArgumentQuery.ArgQuery;
                    ctx.Database.GetDbConnection().Open();

                    using (var reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            var key = reader.GetValue(0);
                            var value = reader.GetValue(1);
                            pairs.Add(key, value);
                        }
                    }
                    ctx.Database.GetDbConnection().Close();
                }

                return pairs;
            }
            return null;
        }

        public ReportArgument GetReportArgument(int id) {
            var arg = ctx.ReportArguments.Include(y => y.ReportArgumentQuery).Where(p => p.ReportArgumentId.Equals(id)).SingleOrDefault();
            if (arg != null) {
                arg.ArgValues = GetArgumentPairs(arg);
            }
            return arg;
        }

        public IEnumerable<ReportArgumentQuery> GetReportArgumentQueries() {
            return ctx.ReportArgumentQuerys.ToList();
        }

        public ReportArgumentQuery GetReportArgumentQuery(int id) {
            return ctx.ReportArgumentQuerys.Where(p => p.ReportArgumentQueryId.Equals(id)).SingleOrDefault();
        }

        public async Task<ReportResult> ExecuteReport(string name, IQueryCollection args) {
            var report = GetReport(name);
            if (report == null) {
                return null;
            }

            ReportResult result;

            IList<ReportRow> rows = new List<ReportRow>();

            using (var cmd = ctx.Database.GetDbConnection().CreateCommand()) {
                cmd.CommandText = name;
                cmd.CommandType = CommandType.StoredProcedure;

                await ctx.Database.GetDbConnection().OpenAsync();

                var argList = new List<string>();
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

                using (var reader = await cmd.ExecuteReaderAsync()) {
                    result = new ReportResult(name);
                    var schema = reader.GetSchemaTable();

                    foreach (DataRow myField in schema.Rows) {
                        var col = new ReportColumn {
                            Name = myField[schema.Columns.IndexOf("ColumnName")].ToString(),
                            DataType = myField[schema.Columns.IndexOf("DataType")].ToString(),
                            Ordinal = (int)myField[schema.Columns.IndexOf("ColumnOrdinal")] - 1
                        };
                        result.Columns.Add(col);
                    };

                    while (reader.Read()) {
                        var row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        result.Rows.Add(row);
                    }
                }
            }
            return result;
        }
    }
}
