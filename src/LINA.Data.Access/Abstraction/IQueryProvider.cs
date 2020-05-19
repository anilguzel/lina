using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LINA.Data.Access.Abstraction
{
    public interface IQueryProvider
    {
        IQueryable<T> Query<T>()
            where T : class;
    }
}
