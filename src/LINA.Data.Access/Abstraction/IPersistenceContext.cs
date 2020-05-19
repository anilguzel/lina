using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LINA.Data.Access.Abstraction
{
    public interface IPersistenceContext : IQueryProvider, ICacheProvider
    {
        void Add(object entity);

        void AddRange(IEnumerable<object> entities);

        void AddRange(params object[] entities);

        void Update(object entity);

        void UpdateRange(IEnumerable<object> entities);

        void UpdateRange(params object[] entities);

        void Remove(object entity);

        void RemoveRange(IEnumerable<object> entities);

        void RemoveRange(params object[] entities);

        void SaveChanges();

        Task SaveChangesAsync(
            CancellationToken cancellationToken = default);

        Task<(bool success, int count)> SaveChangesWithCatchAsync(
            CancellationToken cancellationToken = default);

        Task SaveChangesWithCatchAndThrowAsync(
            CancellationToken cancellationToken = default);
    }
}
