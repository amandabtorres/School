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
    [Authorize]
    public class ClassSchoolController : Controller
    {
        private readonly IClassSchoolRepository _classSchoolRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IUserHelper _userHelper;
        private readonly IFlashMessage _flashMessage;

        public ClassSchoolController(
            IClassSchoolRepository classSchoolRepository,
            ISubjectRepository subjectRepository,
            IUserHelper userHelper,
            IFlashMessage flashMessage)
        {
            _classSchoolRepository = classSchoolRepository;
            _subjectRepository = subjectRepository;
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
                Teachers = await _classSchoolRepository.GetComboTeachersAsync(),
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
                        Subject = subject,
                        TeacherId = teacher.Id,
                        Teacher = teacher,
                    };
                    classSchool.Subjects.Add(subjectClassDetail);
                    await _classSchoolRepository.UpdateAsync(classSchool);                                
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
            
            var classId = await _classSchoolRepository.DeleteSubjectClassDetailAsync(id.Value);
            return RedirectToAction($"SubjectsInClass", new { id = classId });
        }

        

        public IActionResult ClassNotFound()
        {
            return View();
        }


    }    
}
