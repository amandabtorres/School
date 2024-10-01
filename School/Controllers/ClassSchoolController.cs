using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Data;
using School.Data.Entities;
using School.Helpers;
using School.Models;

namespace School.Controllers
{
    public class ClassSchoolController : Controller
    {
        private readonly IClassSchoolRepository _classSchoolRepository;

        public ClassSchoolController(IClassSchoolRepository classSchoolRepository)
        {
            _classSchoolRepository = classSchoolRepository;
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





        public IActionResult ClassNotFound()
        {
            return View();
        }




    }    
}
