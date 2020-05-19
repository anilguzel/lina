using System;
using System.Collections.Generic;
using System.Text;
using LINA.Data.Model.Abstraction;

namespace LINA.Data.Model
{
    public class Pseudo : IEntity<int>
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }

        public string Name { get; set; }
    }
}
