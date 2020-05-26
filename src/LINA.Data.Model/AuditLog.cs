using System;
using System.Collections.Generic;
using System.Text;

namespace LINA.Data.Model
{
    public class AuditLog
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public string Event { get; set; }

        public DateTime EventDate { get; set; }

        public string TableName { get; set; }
        
        public string ColumnName { get; set; }
        
        public string OriginalValue { get; set; }
        
        public string NewValue { get; set; }
    }
}
