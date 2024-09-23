using School.Data.Entities;

namespace School.Data
{
    public interface IUserRepository
    {
        void Add(User user);
        void Delete(User user);
        IEnumerable<User> GetAll();
        Task<bool> SaveAllAsync();
        void Update(User user);
        User GetUserById(string id);

        User GetUserByEmail(string email);

        bool UserEmailExist(string email);
    }
}