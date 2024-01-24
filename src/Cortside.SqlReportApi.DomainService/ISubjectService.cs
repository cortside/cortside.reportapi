using System.Threading.Tasks;
using Cortside.SqlReportApi.Dto;

namespace Cortside.SqlReportApi.DomainService {
    public interface ISubjectService {
        Task SaveAsync(SubjectDto subject);
    }
}
