using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using School.Data.Entities;
using School.Models;
using System.Threading.Tasks;

namespace School.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<User> GetUserByIdAsync(string userId);

        Task<IdentityResult> AddUserWithPasswordAsync(User user, string password);
        Task<IdentityResult> AddUserAsync(User user);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();

        Task<IdentityResult> UpdateUserAsync(User user);

        Task<IdentityResult> DeleteUserAsync(User user);

        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);

        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(User user, string roleName);

        IEnumerable<SelectListItem> GetComboRoles();

        Task<string> GenerateEmailConfirmationTokenAsync(User user);

        Task<IdentityResult> ConfirmEmailAsync(User user, string token);

        Task<string> GeneratePasswordResetTokenAsync(User user);

        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);


        //----------




        Task<SignInResult> ValidatePasswordAsync(User user, string password);

        

        Task<bool> IsUserInRoleAsync(User user, string roleName);
    }
}
