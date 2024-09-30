using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using School.Data.Entities;
using System.Diagnostics.Metrics;

namespace School.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Subject> Subjects { get; set; }

        public DbSet<ClassSchool> ClassSchool { get; set; }

        public DbSet<StudentsClassDetail> StudentsClassDetails { get; set; }

        public DbSet<SubjectsClassDetail> SubjectsClassDetails { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentsClassDetail>()
                 .Property(p => p.Grade)
                 .HasColumnType("decimal(4,2)");

            base.OnModelCreating(modelBuilder);
        }
    }
}
