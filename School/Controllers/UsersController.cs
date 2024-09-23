using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using School.Data;
using School.Data.Entities;
using School.Helpers;
using School.Models;
using System.Net;

namespace School.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserHelper _userHelper;

        public UsersController(IUserRepository userRepository, IUserHelper userHelper)
        {
            _userRepository = userRepository;
            _userHelper = userHelper;
        }
        public IActionResult Index()
        {
            return View(_userRepository.GetAll());
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userRepository.GetUserByEmail(model.Username);
                if (user == null) 
                {
                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        DateBirth = model.DateBirth,
                        Email = model.Username,                        
                        Address = model.Address,
                        PhoneNumber = model.PhoneNumber,
                        PostalCode = model.PostalCode,
                        Nif = model.Nif
                    };
                     _userRepository.Add(user);
                    await _userRepository.SaveAllAsync();
                    return RedirectToAction("Index");
                }
                //avisar que o email ja esta sendo usado
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var user = _userRepository.GetUserById(id);
            var model = new UserViewModel();
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
        public async Task<IActionResult> Edit(UserViewModel model)
        {            
            if (ModelState.IsValid)
            {   
                var user = _userRepository.GetUserByEmail(model.Username);
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.DateBirth = model.DateBirth;                
                user.Address = model.Address;
                user.PhoneNumber = model.PhoneNumber;
                user.PostalCode = model.PostalCode;
                user.Nif = model.Nif;
                try
                {   
                    _userRepository.Update(user);
                    await _userRepository.SaveAllAsync();
                }
                catch (Exception)
                {
                    return NotFound();
                }
                return RedirectToAction("Index");               
                
            }   
            return View(model);
        }

        public IActionResult Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _userRepository.GetUserById(id);
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
            var user = _userRepository.GetUserById(id);
            _userRepository.Delete(user);
            await _userRepository.SaveAllAsync();
            return RedirectToAction("Index");
        }
    }
}
