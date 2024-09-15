
using School.Data.Entities;

namespace School.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            if (!_context.Subjects.Any())
            {
                AddSubject("Português", "UFDC 5062", 50);
                AddSubject("Matemática", "UFDC 5064", 50);
                AddSubject("Língua Inglesa", "UFDC 5063", 50);
                AddSubject("Física e Química", "UFDC 8886", 25);

                await _context.SaveChangesAsync();
            }
        }

        private void AddSubject(string name, string description, int workload)
        {
            _context.Subjects.Add(new Subject
            {
                Name = name,
                Description = description,
                Workload = workload
            });
        }
    }
}
