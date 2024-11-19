using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using tasker_app.Models.User;
using tasker_app.DataLayer.Utils;
using tasker_app.Models;

namespace tasker_app.DBContexts
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options)
           : base(options)
        {
        }
        public DbSet<BaseUser> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Users tables config

            modelBuilder.Entity<BaseUser>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<BaseUser>()
                .HasDiscriminator(u => u.UserType)
                .HasValue<AdminUser>(UserType.AdminUser);

            modelBuilder.Entity<AdminUser>()
                .Property(b => b.FirstName)
                .HasColumnName("FirstName");


            #endregion
            #region DataSeed

            #endregion
        }
    }
}
