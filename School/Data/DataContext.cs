using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using School.Data.Entities;

namespace School.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Subject> Subjects { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
    }
}
