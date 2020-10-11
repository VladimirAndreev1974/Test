using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AtsenergoGrabber.Core
{
    public class ReportContext : DbContext
    {
        public ReportContext()
            : base("DefaultConnection")
        { }

        public DbSet<Loss> Losses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Loss>()
                .Property(c => c.ReportDate).HasMaxLength(10);

            modelBuilder.Entity<Loss>().Property(p => p.Amount)
                .HasPrecision(8, 3);

        }
    }
}