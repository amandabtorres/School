using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using School.Data.Entities;
using School.Helpers;
using School.Migrations;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace School.Data
{
    public class ClassSchoolRepository : GenericRepository<ClassSchool>, IClassSchoolRepository
    {
        private readonly DataContext _context;

        public ClassSchoolRepository(DataContext context) : base(context)
        {
            _context = context;
        }             

        public async Task<IEnumerable<SubjectsClassDetail>> GetSubjectsInClassAsync(int idClass)
        {
            var classSchool =  await _context.ClassSchool.FindAsync(idClass);
            if (classSchool == null) 
            {
                return null;
            }

            return _context.SubjectsClassDetails
                .Include(s => s.Subject)
                .Include(u=> u.Teacher)
                .Where(c => c.ClassSchoolId == classSchool.Id).AsNoTracking();
        }

        public async Task<IEnumerable<SelectListItem>> GetComboSubjectsNotInClassAsync(int idClass)
        {
            var classSchool = await _context.ClassSchool.FindAsync(idClass);
            if (classSchool == null)
            {
                return null;
            }
            var subjectsInClass = await _context.SubjectsClassDetails
                .Where(s => s.ClassSchoolId == idClass)
                .Select(s => s.SubjectId).ToListAsync();

            var subjectsNotInClass = await _context.Subjects
                .Where(s => !subjectsInClass.Contains(s.Id))
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                })
            .ToListAsync();

            subjectsNotInClass.Insert(0, new SelectListItem
            {
                Text = "(Select a subject...)",
                Value = "0"
            });

            return subjectsNotInClass;
        }       

        public SubjectsClassDetail GetSubjectClassDetail(int id)
        {
            return _context.SubjectsClassDetails.FirstOrDefault(s => s.Id == id);
        }


        public async Task<Response> AddSubjectClassDetailInClass(SubjectsClassDetail subjectClassDetail, ClassSchool classSchool)
        {            
            try
            {
                await _context.SubjectsClassDetails.AddAsync(subjectClassDetail);
                await _context.SaveChangesAsync();
                var scd = _context.SubjectsClassDetails
                    .Where(s => s.SubjectId == subjectClassDetail.SubjectId && s.ClassSchoolId == classSchool.Id).First();
 
                var listStudentsInClass = await GetStudentsInClassAsync(classSchool.Id);
                if(listStudentsInClass.Count() > 0)
                {
                    foreach (var student in listStudentsInClass)
                    {
                        var newScd = new StudentsClassDetail
                        {
                            StudentId = student.Id,
                            SubjectsClassDetailId = scd.Id
                        };
                        await _context.StudentsClassDetails.AddAsync(newScd);
                    }
                }                
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "An error occurred in the process: " + ex.Message,
                };
            }
            return new Response { IsSuccess = true, Message = "The subject was added to class" };
        }

        public async Task<Response> RemoveSubjectClassDetailAsync(SubjectsClassDetail scd)
        {           
            var classSchool = await _context.ClassSchool.FindAsync(scd.ClassSchoolId);
            if (classSchool == null) 
            {
                return new Response { Message = "Class Not Found!" };
            }
            else
            {
                if (classSchool.StartDate < DateTime.Now)
                {
                    return new Response {Message = "It is not possible to remove a subject from a class that has already started... " };
                }
            }     
            _context.SubjectsClassDetails.Remove(scd);
            await _context.SaveChangesAsync();
            return new Response { Message = "Subject was removed from class" };
        }


        public async Task<IEnumerable<User>> GetStudentsInClassAsync(int idClass)
        {
            var classSchool = await _context.ClassSchool.FindAsync(idClass);
            if (classSchool == null)
            {
                return null;
            }
            return await _context.StudentsClassDetails
                .Include(s => s.Student)
                .Include(x => x.SubjectsClassDetail)
                .Where(su => su.SubjectsClassDetail.ClassSchoolId == idClass)
                .GroupBy(s => s.StudentId)
                .Select(g => g.First().Student) // Seleciona o primeiro aluno de cada grupo
                .ToListAsync();

        }

        public async Task<IEnumerable<User>> GetStudentsNotEnrolledInClassAsync(int idClass)
        {
            var classSchool = await _context.ClassSchool.FindAsync(idClass);
            if (classSchool == null)
            {
                return null;
            }

            string sqlQuery = "select u.* from AspNetUsers u " +
                "inner join AspNetUserRoles r " +
                "on u.Id=r.UserId " +
                "inner join AspNetRoles roles " +
                "on roles.Id=r.RoleId " +
                "left join (" +
                "select distinct t1.StudentId " +
                "from StudentsClassDetails t1 " +
                "inner join SubjectsClassDetails t2 on " +
                "t1.SubjectsClassDetailId = t2.Id " +
                "where t2.ClassSchoolId <> " + idClass + ") scd " +
                "on scd.StudentId = u.Id " +
                "where roles.Name='Student' " +
                "and u.id not in (select t1.StudentId from StudentsClassDetails t1 inner join SubjectsClassDetails t2 on t1.SubjectsClassDetailId = t2.Id where t2.ClassSchoolId = " + idClass + ")";

            var list = await _context.Users.FromSql(FormattableStringFactory.Create(sqlQuery)).ToListAsync();

            return list;
        }

        public async Task<Response> AddStudentInClass(User user, ClassSchool classSchool)
        {
            try
            {
                var listSubjectsClass = await GetSubjectsInClassAsync(classSchool.Id);
                if(listSubjectsClass.Count() > 0)
                {
                    foreach (var subjectClass in listSubjectsClass)
                    {
                        var scd = new StudentsClassDetail
                        {
                            StudentId = user.Id,
                            SubjectsClassDetailId = subjectClass.Id,
                            Absence = 0,
                        };
                        await _context.StudentsClassDetails.AddAsync(scd);
                    };
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return new Response { IsSuccess = true, Message = "It's necessary add subjects in class, before add student!" };
                }     
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "An error occurred in the process: " + ex.Message,
                };
            }
            return new Response { IsSuccess = true, Message = "The student was added to class" };
        }

        public async Task<Response> RemoveStudentInClass(User user, ClassSchool classSchool)
        {
            if(classSchool.StartDate < DateTime.Now)
            {
                return new Response{Message = "It is not possible to remove a student from a class that has already started... " };
            }
            try
            {   
                var listStudentClassDetail = await _context.StudentsClassDetails
                    .Where(scd => scd.StudentId == user.Id && scd.SubjectsClassDetail.ClassSchoolId == classSchool.Id).ToListAsync();

                foreach (var scd in listStudentClassDetail)
                {
                    _context.StudentsClassDetails.Remove(scd);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                return new Response
                {
                    IsSuccess = false,
                    Message = "An error occurred in the process: " + ex.Message,
                };
            }
            return new Response { IsSuccess = true, Message = "The student was removed from class" };
        }


    }
}
