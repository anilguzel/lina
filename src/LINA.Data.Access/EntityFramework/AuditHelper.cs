using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINA.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Pluralize.NET.Core;

namespace LINA.Data.Access.EntityFramework
{
    public static class AuditHelper
    {
        public static async Task<List<AuditLog>> GetAuditRecordsForChangeAsync(EntityEntry dbEntry, string userId)
        {
            List<AuditLog> result = new List<AuditLog>();

            DateTime changeTime = DateTime.UtcNow;

            // Get the Table() attribute, if one exists
            var tableAttr = dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), false).SingleOrDefault() as TableAttribute;

            // Get table name (if it has a Table attribute, use that, otherwise get the pluralized name)
            var pluralizer = new Pluralizer();
            string tableName = tableAttr != null ? tableAttr.Name : pluralizer.Pluralize(dbEntry.Entity.GetType().Name);

            if (dbEntry.State == EntityState.Added)
            {
                foreach (var property in dbEntry.Properties)
                {
                    result.Add(new AuditLog()
                    {
                        Id = Guid.NewGuid(),
                        UserId = userId.ToString(),
                        EventDate = changeTime,
                        Event = "A",    // Modified
                        TableName = tableName,
                        ColumnName = property.Metadata.Name,
                        NewValue = property.OriginalValue?.ToString()
                    });

                }

            }
            else if (dbEntry.State == EntityState.Deleted)
            {
                // Same with deletes, do the whole record, and use either the description from Describe() or ToString()
                result.Add(new AuditLog()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId.ToString(),
                    EventDate = changeTime,
                    Event = "D", // Deleted
                    TableName = tableName,
                    ColumnName = "*ALL",
                    NewValue = dbEntry.OriginalValues.ToObject().ToString()
                });
            }
            else if (dbEntry.State == EntityState.Modified)
            {
                foreach (var property in dbEntry.Properties)
                {
                    // For updates, we only want to capture the columns that actually changed
                    if (!property.OriginalValue.Equals(property.CurrentValue))
                    {
                        result.Add(new AuditLog()
                        {
                            Id = Guid.NewGuid(),
                            UserId = userId.ToString(),
                            EventDate = changeTime,
                            Event = "M",    // Modified
                            TableName = tableName,
                            ColumnName = property.Metadata.Name,
                            OriginalValue = property.OriginalValue?.ToString(),
                            NewValue = property.CurrentValue?.ToString()
                        });
                    }

                }
            }

            return result;
        }
    }
}
