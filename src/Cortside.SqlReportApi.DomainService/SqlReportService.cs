using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cortside.SqlReportApi.Data;
using Cortside.SqlReportApi.Domain.Entities;
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

        public IList<Report> GetReports() {
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

        public IList<ReportGroup> GetReportGroups() {
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

        public IList<ReportArgument> GetReportArguments() {
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

        public IList<ReportArgumentQuery> GetReportArgumentQueries() {
            return db.ReportArgumentQuerys.ToList();
        }

        public ReportArgumentQuery GetReportArgumentQuery(int id) {
            return db.ReportArgumentQuerys.Where(p => p.ReportArgumentQueryId.Equals(id)).SingleOrDefault();
        }

        public async Task<ReportResult> ExecuteReportAsync(string name, IQueryCollection args, List<string> permissions) {
            var report = GetReport(name);
            if (report == null) {
                throw new ResourceNotFoundMessage($"Report {name} could not be found.");
            }

            // TODO: for testing
            //if (!permissions.Contains(report.Permission)) {
            //    throw new NotAuthorizedMessage($"The requested resource requires the permission: {report.Permission}.");
            //}

            ReportResult result = new ReportResult(name);

            using (var cmd = db.Database.GetDbConnection().CreateCommand()) {
                cmd.CommandText = name;
                cmd.CommandType = CommandType.StoredProcedure;

                await db.Database.GetDbConnection().OpenAsync().ConfigureAwait(false);

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

                    using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false)) {
                        var rs = 0;
                        do {
                            Console.WriteLine("Result set: {0}", ++rs);
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
                } catch (Exception ex) {
                    logger.LogError(ex, "Exception occured when requesting report from database");
                    throw;
                } finally {
                    await db.Database.GetDbConnection().CloseAsync().ConfigureAwait(false);
                }
            }
            return result;
        }

        public Stream ExportReport(ReportResult report) {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            foreach (var resultset in report.ResultSets) {
                // write header
                foreach (var column in resultset.Columns) {
                    writer.Write($"{column.Name},");
                }

                // write body
                foreach (var row in resultset.Rows) {
                    writer.WriteLine();
                    foreach (var column in row) {
                        if (column.ToString().Contains(',')) {
                            // handle commas
                            writer.Write($"\"{column}\",");
                        } else {
                            writer.Write($"{column},");
                        }
                    }
                }
                writer.WriteLine();
            }
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
