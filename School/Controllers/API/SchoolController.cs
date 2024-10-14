using Microsoft.AspNetCore.Mvc;
using School.Data;


namespace School.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SchoolController : Controller
    {
        private readonly IClassSchoolRepository _classSchoolRepository;
        private readonly IStudentsClassDetailRepository _studentsClassDetailRepository;
        
        public SchoolController(
            IClassSchoolRepository classSchoolRepository,
            IStudentsClassDetailRepository studentsClassDetailRepository)
        {
            _classSchoolRepository = classSchoolRepository;
            _studentsClassDetailRepository = studentsClassDetailRepository;
        }

        [HttpGet]
        public IActionResult GetClassSchool()
        {
            return Ok(_classSchoolRepository.GetAll());
        }

        [HttpGet("{idClass}")]
        public async Task<IActionResult> GetStudentsClass(int idClass)
        {
            return Ok(await _classSchoolRepository.GetStudentsInClassAsync(idClass));
        }

        [HttpGet("{idScd}")]
        public async Task<IActionResult> GetStudentsClassDetailByScd(int idScd)
        {
            return Ok(await _studentsClassDetailRepository.GetStudentsClassDetailsByIdSubjectClassDetailAsync(idScd));
        }

        [HttpGet("{idClass}")]
        public async Task<IActionResult> GetSubjectsClassDetailByClass(int idClass)
        {
            return Ok(await _classSchoolRepository.GetSubjectsInClassAsync(idClass));
        }

        [HttpGet]
        public IActionResult GetClassSchoolDetails()
        {
            return Ok(_classSchoolRepository.GetAll());
        }


    }
}
