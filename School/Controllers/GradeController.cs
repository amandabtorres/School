using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Data;
using School.Data.Entities;
using School.Helpers;
using Vereyon.Web;

namespace School.Controllers
{
    [Authorize(Roles = "Teacher, Admin")]
    public class GradeController : Controller
    {
        private readonly IStudentsClassDetailRepository _studentsClassDetailRepository;
        private readonly IClassSchoolRepository _classSchoolRepository;
        private readonly IUserHelper _userHelper;
        private readonly IFlashMessage _flashMessage;

        public GradeController(
            IStudentsClassDetailRepository studentsClassDetailRepository,
            IClassSchoolRepository classSchoolRepository,
            IUserHelper userHelper,
            IFlashMessage flashMessage)
        {
            _studentsClassDetailRepository = studentsClassDetailRepository;
            _classSchoolRepository = classSchoolRepository;
            _userHelper = userHelper;
            _flashMessage = flashMessage;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _studentsClassDetailRepository.GetSubjectsClassDetailByTeacherAsync(this.User.Identity.Name);

            return View(list);
        }

        public async Task<IActionResult> StudentsBySubjectThrowGrade(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var scd = _classSchoolRepository.GetSubjectClassDetail(id.Value);
            if (scd == null) 
            {
                return NotFound();
            }
            var user = await _userHelper.GetUserByIdAsync(scd.TeacherId);
            if (this.User.Identity.Name != user.Email && !this.User.IsInRole("Admin"))
            {
                return Forbid();
            }
            var list = await _studentsClassDetailRepository.GetStudentsClassDetailsBySubjectClassDetailAsync(scd);
            return View(list);
        }
               
        public async Task<IActionResult> SaveGrade(int id, decimal grade)
        {
            Response result = await _studentsClassDetailRepository.SaveGradeAsync(id, grade);
            if (result.IsSuccess)
            {
                return Ok(new { success = true });
            }
            else
            {
                return BadRequest(new { success = false, message = result.Message });
            }            
        }

        public async Task<IActionResult> StudentsBySubjectThrowAbsence(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var scd = _classSchoolRepository.GetSubjectClassDetail(id.Value);
            if (scd == null)
            {
                return NotFound();
            }
            var user = await _userHelper.GetUserByIdAsync(scd.TeacherId);
            if (this.User.Identity.Name != user.Email && !this.User.IsInRole("Admin"))
            {
                return Forbid();
            }
            var list = await _studentsClassDetailRepository.GetStudentsClassDetailsBySubjectClassDetailAsync(scd);
            return View(list);
        }

        public async Task<IActionResult> DecreaseAbsence(int id, int qtd)
        {
            Response result = await _studentsClassDetailRepository.DecreaseAbsenceAsync(id, qtd);
            if (result.IsSuccess)
            {
                return Ok(new { success = true });
            }
            else
            {
                return BadRequest(new { success = false, message = result.Message });
            }
        }

        public async Task<IActionResult> IncreaseAbsence(int id, int qtd)
        {
            Response result = await _studentsClassDetailRepository.IncreaseAbsenceAsync(id, qtd);
            if (result.IsSuccess)
            {
                return Ok(new { success = true });
            }
            else
            {
                return BadRequest(new { success = false, message = result.Message });
            }
        }
    }
}
