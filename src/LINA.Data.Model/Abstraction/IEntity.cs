using System;
using System.Collections.Generic;
using System.Text;

namespace LINA.Data.Model.Abstraction
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
        DateTime CreateDate { get; set; }
        bool IsActive { get; set; }
    }
}
