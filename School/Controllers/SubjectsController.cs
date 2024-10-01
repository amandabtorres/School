using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School.Data;
using School.Data.Entities;
using School.Helpers;

namespace School.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly ISubjectRepository _subjectRepository;

        public SubjectsController(ISubjectRepository subjectRepository)
        {
           
            _subjectRepository = subjectRepository;
        }

        // GET: Subjects
        public IActionResult Index()
        {
            return View(_subjectRepository.GetAll().OrderBy(s => s.Name));
        }

        // GET: Subjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("SubjectNotFound");
            }

            var subject = await _subjectRepository.GetByIdAsync(id.Value);
                
            if (subject == null)
            {
                return new NotFoundViewResult("SubjectNotFound");
            }

            return View(subject);
        }

        // GET: Subjects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Subject subject)
        {
            if (ModelState.IsValid)
            {
                await _subjectRepository.CreateAsync(subject);               
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }

        // GET: Subjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("SubjectNotFound");
            }

            var subject = await _subjectRepository.GetByIdAsync(id.Value);
            if (subject == null)
            {
                return new NotFoundViewResult("SubjectNotFound");
            }
            return View(subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Subject subject)
        {
            if (id != subject.Id)
            {
                return new NotFoundViewResult("SubjectNotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {                                       
                    await _subjectRepository.UpdateAsync(subject);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _subjectRepository.ExistAsync(subject.Id))
                    {
                        return new NotFoundViewResult("SubjectNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }

        // GET: Subjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("SubjectNotFound");
            }

            var subject = await _subjectRepository.GetByIdAsync(id.Value);
            if (subject == null)
            {
                return new NotFoundViewResult("SubjectNotFound");
            }

            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subject = await _subjectRepository.GetByIdAsync(id);
            await _subjectRepository.DeleteAsync(subject);            
            return RedirectToAction(nameof(Index));
        }

        public IActionResult SubjectNotFound()
        {
            return View();
        }
    }
}
