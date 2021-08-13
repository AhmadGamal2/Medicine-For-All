using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace project_4.Models
{
    public partial class GradContext : DbContext
    {
        public GradContext()
            : base("name=GradContext2")
        {
        }

        public virtual DbSet<Drug_Buying> Drug_Buying { get; set; }
        public virtual DbSet<Drug> Drugs { get; set; }
        public virtual DbSet<Has_Drug> Has_Drug { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
