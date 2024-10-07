﻿using School.Data.Entities;
using School.Helpers;

namespace School.Data
{
    public interface IStudentsClassDetailRepository : IGenericRepository<StudentsClassDetail>
    {
        Task<IEnumerable<SubjectsClassDetail>> GetSubjectsClassDetailByTeacherAsync(string userNameTeacher);

        Task<IEnumerable<StudentsClassDetail>> GetStudentsClassDetailsBySubjectClassDetailAsync(SubjectsClassDetail scd);

        Task<Response> SaveGradeAsync(int id, decimal grade);

        Task<IEnumerable<StudentsClassDetail>> GetStudentClassDetailByClassStudentAsync(User user, int classId);

        Task<IEnumerable<ClassSchool>> GetClassesSchoolByStudent(User user);
    }
}
