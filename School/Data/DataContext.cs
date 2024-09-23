using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using School.Data.Entities;
using System.Diagnostics.Metrics;

namespace School.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Subject> Subjects { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>()
        //        .HasIndex(c => c.Email)
        //        .IsUnique();

        //    base.OnModelCreating(modelBuilder);
        //}
    }
}
