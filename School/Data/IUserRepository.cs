using School.Data.Entities;

namespace School.Data
{
    public interface IUserRepository
    {
       
        IEnumerable<User> GetAll();
       
    }
}