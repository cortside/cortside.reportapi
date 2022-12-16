using System.Threading.Tasks;
using Cortside.SqlReportApi.Dto.Dto;

namespace Cortside.SqlReportApi.DomainService {
    public interface ISubjectService {
        Task SaveAsync(SubjectDto subject);
    }
}
