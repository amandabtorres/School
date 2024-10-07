using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Newtonsoft.Json.Linq;
using School.Data;
using School.Data.Entities;
using School.Helpers;
using School.Models;

namespace School.Controllers
{
    public class SearchController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IClassSchoolRepository _classSchoolRepository;
        private readonly IStudentsClassDetailRepository _studentsClassDetailRepository;
        private readonly IUserHelper _userHelper;

        public SearchController(
            IUserRepository userRepository,
            IClassSchoolRepository classSchoolRepository,
            IStudentsClassDetailRepository studentsClassDetailRepository,
            IUserHelper userHelper)
        {
            _userRepository = userRepository;
            _classSchoolRepository = classSchoolRepository;
            _studentsClassDetailRepository = studentsClassDetailRepository;
            _userHelper = userHelper;
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

        [HttpGet]
        public async Task<IActionResult> DetailsStudent(string id)
        {
            var student = await _userHelper.GetUserByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }            
            var listClasses = await _studentsClassDetailRepository.GetClassesSchoolByStudent(student);
            var model = new DetailsStudentViewModel
            {
                User = student,                
                ClassesOfStudent = listClasses
            };
            return View(model);
        }

        public async Task<JsonResult> DetailClassByStudent(int classId, string userId)
        {
            var student = await _userHelper.GetUserByIdAsync(userId);
            var listSCD = await _studentsClassDetailRepository.GetStudentClassDetailByClassStudentAsync(student, classId);
            return Json(listSCD);
        }

        public async Task<IActionResult> DetailsTeacher(string id)
        {
            var teacher = await _userHelper.GetUserByIdAsync(id);
            if(teacher == null)
            {
                return NotFound();
            }
            var listSCD = await _studentsClassDetailRepository.GetSubjectsClassDetailByTeacherAsync(teacher.UserName);
            var model = new DetailsTeacherViewModel
            {
                User = teacher,
                Subjects = listSCD
            };
            return View(model);
        }

        public async  Task<IActionResult> DetailsEmployee(string id)
        {
            var employee = await _userHelper.GetUserByIdAsync(id);
            if (employee == null)
            { 
                return NotFound(); 
            }
            return View(employee);
        }

        public async Task<IActionResult> DetailsStudentByUserName(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            return this.RedirectToAction($"DetailsStudent", new { id = user.Id });

        }

        public async Task<IActionResult> DetailSubjectInClass(int id, string idTeacher)
        {
            var user = await _userHelper.GetUserByIdAsync(idTeacher);
            if (user == null)
            {
                return NotFound();
            }
            var scd = _classSchoolRepository.GetSubjectClassDetail(id);
            if (scd == null)
            {
                return NotFound();
            }
            var list = await _studentsClassDetailRepository.GetStudentsClassDetailsBySubjectClassDetailAsync(scd);
            var model = new DetailsSubjectTeacherViewModel
            {
                User = user,
                StudentsInSubject = list,
            };
            return View(model);
        }

    }
}
