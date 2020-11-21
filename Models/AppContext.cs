using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Online_Shop___DAW.Models
{
    public class AppContext : DbContext
    {
        public AppContext() : base ("DBConnectionString") {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppContext,
                    Online_Shop___DAW.Migrations.Configuration>("DBConnectionString"));
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}