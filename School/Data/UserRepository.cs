using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using School.Data.Entities;

namespace School.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;


        public UserRepository(DataContext context)
        {
            _context = context;
        }        
        public IEnumerable<User> GetAll()
        {
            return _context.Users.OrderBy(u => u.FirstName);
        }       
                
    }
}
