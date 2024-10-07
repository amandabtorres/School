using Microsoft.AspNetCore.Mvc.Rendering;
using School.Data.Entities;

namespace School.Data
{
    public interface IUserRepository
    {
       
        IEnumerable<User> GetAll();

        Task<IEnumerable<SelectListItem>> GetComboTeachersAsync();

        Task<IEnumerable<User>> GetUserByRoleInSchool(string roleName);


    }
}