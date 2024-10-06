using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Data;

namespace School.Controllers
{
    public class SearchController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IClassSchoolRepository _classSchoolRepository;

        public SearchController(
            IUserRepository userRepository,
            IClassSchoolRepository classSchoolRepository)
        {
            _userRepository = userRepository;
            _classSchoolRepository = classSchoolRepository;
        }

        [Authorize(Roles = "Teacher, Admin, Employee")]
        public async Task<IActionResult> Students()
        {
            return View(await _userRepository.GetUserByRoleInSchool("Student"));
        }

        public async Task<IActionResult> Teachers()
        {
            return View(await _userRepository.GetUserByRoleInSchool("Teacher"));
        }

        public async Task<IActionResult> Employees()
        {
            return View(await _userRepository.GetUserByRoleInSchool("Employee"));
        }

        public IActionResult Classes()
        {
            return View(_classSchoolRepository.GetAll());
        }

        

    }
}
