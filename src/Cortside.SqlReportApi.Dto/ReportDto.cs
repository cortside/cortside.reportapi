using System.Collections.Generic;

namespace Cortside.SqlReportApi.Dto {
    public class ReportDto {
        public int ReportId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ReportGroupDto ReportGroup { get; set; }

        public string Permission { get; set; }

        public IList<ReportArgumentDto> ReportArguments { get; set; }

        public ReportDto() {
            ReportArguments = new List<ReportArgumentDto>();
        }
    }
}
