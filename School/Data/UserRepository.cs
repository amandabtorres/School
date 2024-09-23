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

        public void Add(User user)
        {
            _context.Users.Add(user);
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.OrderBy(u => u.FirstName);
        }

        public User GetUserById(string id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);           
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email ==  email);
        }

        public bool UserEmailExist(string email)
        {
            return _context.Users.Any(u =>  u.Email == email);
        }
                
    }
}
