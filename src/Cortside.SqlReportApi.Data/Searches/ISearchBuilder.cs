using System.Linq;

namespace Cortside.SqlReportApi.Data.Searches {
    public interface ISearchBuilder<T> {
        IQueryable<T> Build(IQueryable<T> list);
    }
}
