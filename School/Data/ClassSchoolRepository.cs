using School.Data.Entities;

namespace School.Data
{
    public class ClassSchoolRepository : GenericRepository<ClassSchool>, IClassSchoolRepository
    {
        private readonly DataContext _context;

        public ClassSchoolRepository(DataContext context) : base(context)
        {
            _context = context;
        }

    }
}
