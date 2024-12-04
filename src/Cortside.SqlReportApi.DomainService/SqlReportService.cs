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

        public async Task<Report> GetReportAsync(string name) {
            logger.LogInformation("Retrieving report {Name}.", name);
            var report = await db.Reports
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
            logger.LogInformation("Retrieving all Reports.");
            var reports = await db.Reports
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
            logger.LogInformation("Retrieving all ReportGroups.");
            return await db.ReportGroups.ToListAsync().ConfigureAwait(false);
        }

        public async Task<ReportGroup> GetReportGroupAsync(int id) {
            logger.LogInformation("Retrieving ReportGroup {Id}.", id);
            var reportGroup = await db.ReportGroups.Where(p => p.ReportGroupId.Equals(id)).SingleOrDefaultAsync().ConfigureAwait(false);
            if (reportGroup == null) {
                throw new ResourceNotFoundMessage($"ReportGroup {id} could not be found.");
            }
            return reportGroup;
        }

        public async Task<IList<ReportArgument>> GetReportArgumentsAsync() {
            var args = await db.ReportArguments.Include(y => y.ReportArgumentQuery).ToListAsync().ConfigureAwait(false);
            foreach (var arg in args) {
                arg.ArgValues = GetArgumentPairs(arg);
            }

            return args;
        }

        private Dictionary<string, object> GetArgumentPairs(ReportArgument arg) {
            if (arg?.ReportArgumentQueryId.HasValue == true) {
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

        public async Task<ReportArgument> GetReportArgumentAsync(int id) {
            var arg = await db.ReportArguments.Include(y => y.ReportArgumentQuery).Where(p => p.ReportArgumentId.Equals(id)).SingleOrDefaultAsync().ConfigureAwait(false);
            if (arg != null) {
                arg.ArgValues = GetArgumentPairs(arg);
            }
            return arg;
        }

        public async Task<IList<ReportArgumentQuery>> GetReportArgumentQueriesAsync() {
            return await db.ReportArgumentQuerys.ToListAsync().ConfigureAwait(false);
        }

        public Task<ReportArgumentQuery> GetReportArgumentQueryAsync(int id) {
            return db.ReportArgumentQuerys.Where(p => p.ReportArgumentQueryId.Equals(id)).SingleOrDefaultAsync();
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

            await using (var cmd = db.Database.GetDbConnection().CreateCommand()) {
                cmd.CommandText = name;
                cmd.CommandType = CommandType.StoredProcedure;

                await db.Database.GetDbConnection().OpenAsync().ConfigureAwait(false);

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
