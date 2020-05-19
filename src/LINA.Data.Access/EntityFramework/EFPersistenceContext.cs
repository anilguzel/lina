using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LINA.Core.Infrastructure.Cache.Abstraction;
using LINA.Data.Access.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LINA.Data.Access.EntityFramework
{
    public class EFPersistenceContext : IPersistenceContext
    {
        private readonly ILogger<EFPersistenceContext> _logger;
        private readonly ICacheService _cacheService;

        public EFPersistenceContext(
            ILogger<EFPersistenceContext> logger,
            ICacheService cacheService)
        {
            _cacheService = cacheService;
            _logger = logger;
        }

        public DbContext DbContext { get; }

        public object Find(Type entityType, params object[] keyValues)
        {
            return DbContext.Find(entityType, keyValues);
        }

        public object Find(Type entityType, IEnumerable<object> keyValues)
        {
            return DbContext.Find(entityType, keyValues);
        }

        public IQueryable<T> Query<T>()
            where T : class
        {
            return DbContext.Set<T>();
        }

        public void Attach(object entity)
        {
            DbContext.Attach(entity);
        }

        public void Update(object entity)
        {
            DbContext.Update(entity);
        }

        public void UpdateRange(IEnumerable<object> entities)
        {
            DbContext.UpdateRange(entities);
        }

        public void UpdateRange(params object[] entities)
        {
            DbContext.UpdateRange(entities);
        }

        public void Add(object entity)
        {
            DbContext.Add(entity);
        }

        public void AddRange(IEnumerable<object> entities)
        {
            DbContext.AddRange(entities);
        }

        public void AddRange(params object[] entities)
        {
            DbContext.AddRange(entities);
        }

        public void Remove(object entity)
        {
            DbContext.Remove(entity);
        }

        public void RemoveRange(IEnumerable<object> entities)
        {
            DbContext.RemoveRange(entities);
        }

        public void RemoveRange(params object[] entities)
        {
            DbContext.RemoveRange(entities);
        }

        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<(bool success, int count)> SaveChangesWithCatchAsync(CancellationToken cancellationToken = default)
        {
            int changes = 0;
            try
            {
                changes = await DbContext.SaveChangesAsync(cancellationToken);
                return (true, changes);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(1, ex.ToString(), "Got an error while saving changes in data access layer. ");
                return (false, changes);
            }
        }

        public async Task SaveChangesWithCatchAndThrowAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await DbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(1, ex.Message, "Got an error while saving changes in data access layer. ");
                throw new Exception(ex.Message);
            }
        }

        public DbContext GetContext()
        {
            return DbContext;
        }

        public List<T> GetListFromCache<T>(string cacheKey, TimeSpan? slidingExpiration = null)
            where T : class
        {
            return _cacheService.GetList<T>(cacheKey) ??
                   (List<T>)_cacheService.Set(cacheKey, DbContext.Set<T>().ToList(), null, slidingExpiration);
        }

        public List<T> GetListFromCache<T>(string cacheKey, Expression<Func<T, bool>> predicate, TimeSpan? slidingExpiration = null)
            where T : class
        {
            return _cacheService.GetList<T>(cacheKey) ??
                   (List<T>)_cacheService.Set(cacheKey, DbContext.Set<T>().Where(predicate).ToList(), null, slidingExpiration);
        }
    }
}
