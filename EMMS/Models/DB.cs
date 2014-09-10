using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace EMMS.Models
{
    public class DB : DbContext
    {
        public DbSet<EMdata> EMdatas { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Terminal> Terminals { get; set; }

        public DbSet<Alarm> Alarms { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
                base.OnModelCreating(modelBuilder);
        }
    }
}