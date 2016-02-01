using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using XOUND.Models;

namespace XOUND
{

    public class XOUNDContext : DbContext
    {
        public DbSet<Album> Albums { get; set; }

        public XOUNDContext()
            : base("DefaultConnection")
        {
            InitDbSets();
        }

        public XOUNDContext(string connectionString)
            : base(connectionString)
        {
            InitDbSets();
        }

        private void InitDbSets()
        {
            this.Albums = this.Set<Album>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Album>().ToTable("Albums")
                .HasKey(x => x.ID)
                .Property(x => x.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}