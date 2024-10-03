using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using School.Data;
using School.Data.Entities;
using School.Helpers;
using School.Models;
using System.Net;
using Vereyon.Web;

namespace School.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserHelper _userHelper;
        private readonly IFlashMessage _flashMessage;

        public UsersController(
            IUserRepository userRepository, 
            IUserHelper userHelper,
            IFlashMessage flashMessage)
        {
            _userRepository = userRepository;
            _userHelper = userHelper;
            _flashMessage = flashMessage;
        }
        public IActionResult Index()
        {
            return View(_userRepository.GetAll());
        }                

        [HttpGet]
        public async  Task<IActionResult> Edit(string id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(id);              
                
            var model = new ChangeUserViewModel();
            if (user != null)
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.DateBirth = user.DateBirth;
                model.Username = user.Email;
                model.Address = user.Address;
                model.PhoneNumber = user.PhoneNumber;
                model.PostalCode = user.PostalCode;
                model.Nif = user.Nif;              

                return View(model);
            }
            return NotFound();           
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ChangeUserViewModel model)
        {            
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);                    
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.DateBirth = model.DateBirth;                
                user.Address = model.Address;
                user.PhoneNumber = model.PhoneNumber;
                user.PostalCode = model.PostalCode;
                user.Nif = model.Nif;
               
                var response = await _userHelper.UpdateUserAsync(user);
                if (response.Succeeded)
                {
                    if (this.User.Identity.Name == user.Email)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    ViewBag.UserMessage = "User updated!";
                    //_flashMessage.Info("User updated!");
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                }
            }   
            return View(model);
        }

        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(id);
            if (user == null) 
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userHelper.GetUserByIdAsync(id);
            await _userHelper.DeleteUserAsync(user); 
            return RedirectToAction("Index");
        }


    }
}
