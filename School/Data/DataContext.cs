using Microsoft.EntityFrameworkCore;
using School.Data.Entities;

namespace School.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Subject> Subjects { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
    }
}
