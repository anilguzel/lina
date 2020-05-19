using LINA.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LINA.Data.Access.EntityFramework
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Pseudo> Pseudos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pseudo>()
                .HasIndex(x => x.Id)
                .IsUnique();
        }
    }
}
