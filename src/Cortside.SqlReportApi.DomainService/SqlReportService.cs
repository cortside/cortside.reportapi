using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cortside.SqlReportApi.Data;
using Cortside.SqlReportApi.Data.Repositories;
using Cortside.SqlReportApi.Domain.Entities;
using Cortside.SqlReportApi.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Cortside.SqlReportApi.DomainService
{
    public class SqlReportService : ISqlReportService
    {
        protected IReportRepository repository;
        private readonly ILogger<SqlReportService> logger;

        public SqlReportService(IReportRepository repository, ILogger<SqlReportService> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public Stream ExportReport(ReportResult report)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            foreach (var resultset in report.ResultSets)
            {
                // write header
                foreach (var column in resultset.Columns)
                {
                    writer.Write($"{column.Name},");
                }

                // write body
                foreach (var row in resultset.Rows)
                {
                    writer.WriteLine();
                    foreach (var column in row)
                    {
                        if (column.ToString().Contains(','))
                        {
                            // handle commas
                            writer.Write($"\"{column}\",");
                        }
                        else
                        {
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

        public Task<Report> GetReportAsync(string name)
        {
            return repository.GetReportAsync(name);
        }

        public Task<IList<Report>> GetReportsAsync()
        {
            return repository.GetReportsAsync();
        }

        public Task<IList<ReportGroup>> GetReportGroupsAsync()
        {
            return repository.GetReportGroupsAsync();
        }

        public Task<ReportGroup> GetReportGroupAsync(int id)
        {
            return repository.GetReportGroupAsync(id);
        }

        public Task<IList<ReportArgument>> GetReportArgumentsAsync()
        {
            return repository.GetReportArgumentsAsync();
        }

        public Task<ReportArgument> GetReportArgumentAsync(int id)
        {
            return repository.GetReportArgumentAsync(id);
        }

        public Task<IList<ReportArgumentQuery>> GetReportArgumentQueriesAsync()
        {
            return repository.GetReportArgumentQueriesAsync();
        }

        public Task<ReportArgumentQuery> GetReportArgumentQueryAsync(int id)
        {
            return repository.GetReportArgumentQueryAsync(id);
        }

        public Task<ReportResult> ExecuteReportAsync(string name, IQueryCollection args, List<string> permissions)
        {
            return repository.ExecuteReportAsync(name, args, permissions);
        }
    }
}
