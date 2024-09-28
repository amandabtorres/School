using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using School.Data.Entities;
using School.Helpers;
using School.Models;
using Vereyon.Web;

namespace School.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;
        private readonly IFlashMessage _flashMessage;

        public AccountController(
            IUserHelper userHelper,
            IMailHelper mailHelper,
            IFlashMessage flashMessage)
        {
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _flashMessage = flashMessage;
        }
        
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (this.Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(this.Request.Query["ReturnUrl"].First());
                    }
                    return this.RedirectToAction("Index", "Home");
                }
            }
            this.ModelState.AddModelError(string.Empty, "Failed to login");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }


        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        _flashMessage.Info("Password changed!");
                        return View(model);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found");
                }
            }
            return this.View(model);
        }

        public async Task<IActionResult> ChangeUser()
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var userId = user.Id;

            return RedirectToAction("Edit", "Users", new { id = userId });            
        }

        public IActionResult Register()
        {
            var model = new RegisterNewUserViewModel
            {
                Roles = _userHelper.GetComboRoles()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            model.Roles = _userHelper.GetComboRoles();

            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user == null)
                {
                    var selectedRole = model.Roles.FirstOrDefault(r => r.Value == model.RoleId.ToString());
                    string roleUser = selectedRole.Text;

                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        DateBirth = model.DateBirth,
                        Email = model.Username,
                        UserName = model.Username,
                        Address = model.Address,
                        PhoneNumber = model.PhoneNumber,
                        PostalCode = model.PostalCode,
                        Nif = model.Nif
                    };

                    var result = await _userHelper.AddUserAsync(user, model.Password);
                    if (result != IdentityResult.Success)
                    {
                        _flashMessage.Danger("The user couldn't be created.");                        
                        return View(model);
                    }
                    await _userHelper.AddUserToRoleAsync(user, roleUser);

                    string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                    string tokenLink = Url.Action( "ConfirmEmail", "Account", new
                    {
                        userid = user.Id,
                        token = myToken
                    }, protocol: HttpContext.Request.Scheme);                   

                    Response response = _mailHelper.SendEmail(model.Username, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                        $"To allow the user, " +
                        $"please click in this link: </br></br><a href = \"{tokenLink}\">Confirm Email</a>");

                    if (response.IsSuccess)
                    {
                        _flashMessage.Info("The instructions to allow you user has been sent to email");                        
                        return RedirectToAction("Index", "Users"); 
                    }

                    return View(model);
                }
                _flashMessage.Danger("The user is already being used.");               
                return View(model);
            }            
            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            return View();

        }

        public IActionResult RecoverPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    _flashMessage.Warning("The email doesn't correspond to a registered user.");                    
                    return View(model);
                }
                var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);

                var link = this.Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = myToken }, protocol: HttpContext.Request.Scheme);

                Response response = _mailHelper.SendEmail(model.Email, "School Password Reset", $"<h1>School Password Reset</h1>" +
                    $"To reset the password click in this link:</br></br>" +
                    $"<a href = \"{link}\">Reset Password</a>");

                if (response.IsSuccess)
                {
                    _flashMessage.Info("The instructions to recover your password has been sent to email.");                    
                }

                return View();
            }
            return View(model);
        }

        public IActionResult ResetPassword(string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(model.UserName);
            if (user != null)
            {
                var result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    _flashMessage.Info("Password reset successful");                    
                    return View();
                }

                _flashMessage.Danger("Error while resetting the password.");                
                return View(model);
            }
            _flashMessage.Danger("User not found.");            
            return View(model);
        }


    }
}
