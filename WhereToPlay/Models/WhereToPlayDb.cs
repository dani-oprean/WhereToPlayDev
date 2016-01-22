using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using WhereToPlay.Models.DB;

namespace WhereToPlay.Models
{
    public class WhereToPlayDb:DbContext
    {
        public WhereToPlayDb():base("name=WhereToPlayDb")
        {
            //this.Configuration.LazyLoadingEnabled = true;
        }

        public DbSet<Sport> Sports { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<ReservationTime> ReservationTimes { get; set; }
        public DbSet<PageAccess> PageAccesses { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Court> Courts { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}