using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Data;
using School.Data.Entities;
using School.Helpers;
using School.Models;
using Vereyon.Web;

namespace School.Controllers
{
    [Authorize(Roles = "Employee, Admin")]
    public class ClassSchoolController : Controller
    {
        private readonly IClassSchoolRepository _classSchoolRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserHelper _userHelper;
        private readonly IFlashMessage _flashMessage;

        public ClassSchoolController(
            IClassSchoolRepository classSchoolRepository,
            ISubjectRepository subjectRepository,
            IUserRepository userRepository,
            IUserHelper userHelper,
            IFlashMessage flashMessage)
        {
            _classSchoolRepository = classSchoolRepository;
            _subjectRepository = subjectRepository;
            _userRepository = userRepository;
            _userHelper = userHelper;
            _flashMessage = flashMessage;
        }

        public IActionResult Index()
        {
            return View(_classSchoolRepository.GetAll());
        }

        // GET: ClassSchool/Create        
        public IActionResult Create()
        {
            return View();
        }

        // POST: ClassSchool/Create        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClassSchoolModel model)
        {
            if (ModelState.IsValid)
            {
                var classSchool = new ClassSchool
                {
                    Name = model.Name,
                    Description = model.Description,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    
                };

                await _classSchoolRepository.CreateAsync(classSchool);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: ClassSchool/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ClassNotFound");
            }

            var classSchool = await _classSchoolRepository.GetByIdAsync(id.Value);
            if (classSchool == null)
            {
                return new NotFoundViewResult("ClassNotFound");
            }

            var model = new EditClassSchoolModel
            {
                Id = classSchool.Id,
                Name = classSchool.Name,
                Description = classSchool.Description,
                StartDate = classSchool.StartDate,
                EndDate = classSchool.EndDate,
            };               

            return View(model);
        }

        // POST: ClassSchool/Edit/5        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditClassSchoolModel model)
        {
            if (ModelState.IsValid)
            {
                var classSchool = await _classSchoolRepository.GetByIdAsync(model.Id);
                if(classSchool != null)
                {
                    classSchool.Name = model.Name;
                    classSchool.Description = model.Description;
                    classSchool.StartDate = model.StartDate;
                    classSchool.EndDate = model.EndDate;
                    
                    await _classSchoolRepository.UpdateAsync(classSchool);
                    return RedirectToAction(nameof(Index));

                }
                return new NotFoundViewResult("ClassNotFound");         
               
            }
            return View(model);
        }

        // GET: ClassSchool/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ClassNotFound");
            }

            var classSchool = await _classSchoolRepository.GetByIdAsync(id.Value);
            if (classSchool == null)
            {
                return new NotFoundViewResult("ClassNotFound");
            }

            return View(classSchool);
        }

        // POST: ClassSchool/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var listSubjects = await _classSchoolRepository.GetSubjectsInClassAsync(id);
            var studentsInClass = await _classSchoolRepository.GetStudentsInClassAsync(id);
            if(listSubjects.Count() > 0 || studentsInClass.Count() > 0)
            {
                _flashMessage.Danger("It is not possible to delete a class that already contains students or subjects!");
                return RedirectToAction(nameof(Index));
            }
            var classSchool = await _classSchoolRepository.GetByIdAsync(id);
            await _classSchoolRepository.DeleteAsync(classSchool);
            return RedirectToAction(nameof(Index));
        }        

        public async Task<IActionResult> SubjectsInClass(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ClassNotFound");
            }
            var listSubjects = await _classSchoolRepository.GetSubjectsInClassAsync(id.Value);

            var model = new SubjectsClassDetailViewModel
            {
                ClassSchoolId = id.Value,
                SubjectsInClass = listSubjects
            };
            return View(model);
        }

        //GET
        public async Task<IActionResult> AddSubject(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ClassNotFound");
            }

            var model = new AddSubjectViewModel
            {
                ClassSchoolId = id.Value,
                Subjects = await _classSchoolRepository.GetComboSubjectsNotInClassAsync(id.Value),
                Teachers = await _userRepository.GetComboTeachersAsync(),
            };
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddSubject(AddSubjectViewModel model)
        {            
            if (ModelState.IsValid)
            {
                var classSchool = await _classSchoolRepository.GetByIdAsync(model.ClassSchoolId);
                var subject = await _subjectRepository.GetByIdAsync(model.SubjectId);
                var teacher = await _userHelper.GetUserByIdAsync(model.TeacherId);
                if(classSchool != null)
                {
                    var subjectClassDetail = new SubjectsClassDetail
                    {
                        ClassSchoolId = classSchool.Id,
                        SubjectId = subject.Id,                        
                        TeacherId = teacher.Id,
                        
                    };                                    
                    Response result = await _classSchoolRepository.AddSubjectClassDetailInClass(subjectClassDetail, classSchool);
                    _flashMessage.Info(result.Message);
                    return this.RedirectToAction($"SubjectsInClass", new { id = classSchool.Id });
                }
                _flashMessage.Danger("Class not found!");
            }
            return View(model);
        }

        public async Task<IActionResult> RemoveSubjectClass(int? id)
        {            
            if (id == null)
            {
                return NotFound();
            }
            var subjectclass = _classSchoolRepository.GetSubjectClassDetail(id.Value);
            if (subjectclass == null)
            {
                return NotFound();
            }               
            var result = await _classSchoolRepository.RemoveSubjectClassDetailAsync(subjectclass);
            _flashMessage.Info(result.Message);
            return RedirectToAction($"SubjectsInClass", new { id = subjectclass.ClassSchoolId });
        }

        public async Task<IActionResult> StudentsInClass(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ClassNotFound");
            }
            var studentsAvailable = await _classSchoolRepository.GetStudentsNotEnrolledInClassAsync(id.Value);
            var studentsInClass = await _classSchoolRepository.GetStudentsInClassAsync(id.Value);

            var model = new StudentsClassDetailViewModel
            {
                ClassSchoolId = id.Value,
                StudentsAvailable = studentsAvailable,
                StudentsInClass = studentsInClass

            };
            return View(model);
        }

        public async Task<IActionResult> AddStudentClass(string? itemId, int? classId)
        {
            if(itemId == null)
            {
                return NotFound();
            }
            var student = await _userHelper.GetUserByIdAsync(itemId);
            if(classId == null)
            {
                return ClassNotFound();
            }
            var classSchool = await _classSchoolRepository.GetByIdAsync(classId.Value);
            Response result = await _classSchoolRepository.AddStudentInClass(student, classSchool);            
            _flashMessage.Info(result.Message);            

            return RedirectToAction($"StudentsInClass", new { id = classSchool.Id });
        }

        public async Task<IActionResult> RemoveStudentClass(string? itemId, int? classId)
        {
            if (itemId == null)
            {
                return NotFound();
            }
            var student = await _userHelper.GetUserByIdAsync(itemId);
            if (classId == null)
            {
                return ClassNotFound();
            }
            var classSchool = await _classSchoolRepository.GetByIdAsync(classId.Value);
            
            Response result = await _classSchoolRepository.RemoveStudentInClass(student, classSchool);
            _flashMessage.Warning(result.Message);

            return RedirectToAction($"StudentsInClass", new { id = classSchool.Id });
        }       

        


        public IActionResult ClassNotFound()
        {
            return View();
        }


    }    
}
