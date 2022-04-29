using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cortside.SqlReportApi.Data;
using Cortside.SqlReportApi.Domain;
using Cortside.SqlReportApi.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cortside.SqlReportApi.DomainService {

    public class SqlReportService : ISqlReportService {
        protected DatabaseContext db;
        private readonly ILogger<SqlReportService> logger;

        public SqlReportService(DatabaseContext db, ILogger<SqlReportService> logger) {
            this.db = db;
            this.logger = logger;
        }

        public Report GetReport(string name) {
            logger.LogInformation($"Retriving report {name}.");
            var report = db.Reports
                .Include(r => r.ReportArguments)
                .ThenInclude(y => y.ReportArgumentQuery)
                .Include(g => g.ReportGroup)
                .Where(p => p.Name.Equals(name)).SingleOrDefault();
            if (report != null) {
                foreach (var arg in report.ReportArguments) {
                    arg.ArgValues = GetArgumentPairs(arg);
                }
            } else {
                throw new ResourceNotFoundMessage($"Report {name} could not be found.");
            }
            return report;
        }

        public IEnumerable<Report> GetReports() {
            logger.LogInformation($"Retriving all Reports.");
            var reports = db.Reports
                .Include(r => r.ReportArguments)
                .ThenInclude(y => y.ReportArgumentQuery)
                .Include(g => g.ReportGroup)
                .ToList();
            foreach (var report in reports) {
                foreach (var arg in report.ReportArguments) {
                    arg.ArgValues = GetArgumentPairs(arg);
                }
            }
            return reports;
        }

        public IEnumerable<ReportGroup> GetReportGroups() {
            logger.LogInformation($"Retriving all ReportGroups.");
            return db.ReportGroups.ToList();
        }

        public ReportGroup GetReportGroup(int id) {
            logger.LogInformation($"Retriving ReportGroup {id}.");
            var reportGroup = db.ReportGroups.Where(p => p.ReportGroupId.Equals(id)).SingleOrDefault();
            if (reportGroup == null) {
                throw new ResourceNotFoundMessage($"ReportGroup {id} could not be found.");
            }
            return reportGroup;
        }

        public IEnumerable<ReportArgument> GetReportArguments() {
            var args = db.ReportArguments.Include(y => y.ReportArgumentQuery).ToList();
            foreach (var arg in args) {
                arg.ArgValues = GetArgumentPairs(arg);
            }

            return args;
        }

        protected Dictionary<string, object> GetArgumentPairs(ReportArgument arg) {
            if (arg != null && arg.ReportArgumentQueryId.HasValue) {
                var pairs = new Dictionary<string, object>();

                using (var cmd = db.Database.GetDbConnection().CreateCommand()) {
                    cmd.CommandText = arg.ReportArgumentQuery.ArgQuery;
                    db.Database.GetDbConnection().Open();
                    try {
                        using (var reader = cmd.ExecuteReader()) {
                            while (reader.Read()) {
                                var key = reader.GetString(0);
                                var value = reader.GetValue(1);
                                pairs.Add(key, value);
                            }
                        }
                    } catch (Exception ex) {
                        logger.LogError(ex, "Exception occured when requesting report from database");
                        throw;
                    } finally {
                        db.Database.GetDbConnection().Close();
                    }
                }

                return pairs;
            }
            return null;
        }

        public ReportArgument GetReportArgument(int id) {
            var arg = db.ReportArguments.Include(y => y.ReportArgumentQuery).Where(p => p.ReportArgumentId.Equals(id)).SingleOrDefault();
            if (arg != null) {
                arg.ArgValues = GetArgumentPairs(arg);
            }
            return arg;
        }

        public IEnumerable<ReportArgumentQuery> GetReportArgumentQueries() {
            return db.ReportArgumentQuerys.ToList();
        }

        public ReportArgumentQuery GetReportArgumentQuery(int id) {
            return db.ReportArgumentQuerys.Where(p => p.ReportArgumentQueryId.Equals(id)).SingleOrDefault();
        }

        public async Task<ReportResult> ExecuteReport(string name, IQueryCollection args, List<string> permissions) {
            var report = GetReport(name);
            if (report == null) {
                throw new ResourceNotFoundMessage($"Report {name} could not be found.");
            }

            // TODO: for testing
            //if (!permissions.Contains(report.Permission)) {
            //    throw new NotAuthorizedMessage($"The requested resource requires the permission: {report.Permission}.");
            //}

            ReportResult result;

            IList<ReportRow> rows = new List<ReportRow>();

            using (var cmd = db.Database.GetDbConnection().CreateCommand()) {
                cmd.CommandText = name;
                cmd.CommandType = CommandType.StoredProcedure;

                await db.Database.GetDbConnection().OpenAsync();

                try {
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
                } catch (Exception ex) {
                    logger.LogError(ex, "Exception occured when requesting report from database");
                    throw;
                } finally {
                    await db.Database.GetDbConnection().CloseAsync();
                }
            }
            return result;
        }

        public Stream ExportReport(ReportResult report) {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            // write header
            foreach (var column in report.Columns) {
                writer.Write($"{column.Name},");
            }

            // write body
            foreach (var row in report.Rows) {
                writer.WriteLine();
                foreach (var column in row) {
                    if (column.ToString().Contains(',')) {
                        // handle commas
                        writer.Write($"\"{column}\",");
                    } else {
                        writer.Write($"{column},");
                    }
                }
            };
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
