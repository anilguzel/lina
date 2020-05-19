using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LINA.Data.Access.Abstraction
{
    public interface ICacheProvider
    {
        List<T> GetListFromCache<T>(string cacheKey, TimeSpan? slidingExpiration = null)
            where T : class;

        List<T> GetListFromCache<T>(string cacheKey, Expression<Func<T, bool>> predicate, TimeSpan? slidingExpiration = null)
            where T : class;
    }
}
