using System.Collections.Generic;

namespace Cortside.SqlReportApi.Dto {
    public class ReportArgumentDto {
        public string Name { get; set; }

        public string ArgName { get; set; }

        public Dictionary<string, object> ArgValues { get; set; }

        public string ArgType { get; set; }

        public int Sequence { get; set; }
    }
}
