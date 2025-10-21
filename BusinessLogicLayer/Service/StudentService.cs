using BusinessLogicLayer.IService;
using DataAccessLayer.Context;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Service
{
    public class StudentService : IStudentService
    {
        private readonly UniversityContext _Context;
        public StudentService(UniversityContext context)
        {
            _Context = context;
        }

        public List<Student> GetAllStudents() =>
            _Context.Students.Include(s => s.Department).Include(s => s.StudentCourses).ToList();

        public List<StudentCourse> GetAllStudentCourses(int studentId)
        {
            return _Context.StudentCourses.Include(sc => sc.Course).Where(sc => sc.StudentId == studentId).ToList();
        }

        public Student? GetStudentById(int SSN) => _Context.Students.Include(s => s.Department).FirstOrDefault(s => s.SSN == SSN);

        public bool AddStudent(Student student)
        {
            _Context.Students.Add(student);
            return _Context.SaveChanges() > 0;
        }

        public bool UpdateStudent(Student student)
        {

            _Context.Students.Update(student);
            if (_Context.SaveChanges() > 0)
            {
                return true;
            }
            else
                return false;
        }

        public bool DeleteStudent(Student student)
        {
            _Context.Students.Remove(student);
            return _Context.SaveChanges() > 0;
        }

        ////////////////////////////////////////// Student-Course Management Section //////////////////////////////////////////

        public StudentCourse GetStudentCourse(int StudentId)
        {
            return (_Context.StudentCourses.FirstOrDefault(sc => sc.StudentId == StudentId));
        }

        public StudentCourse GetStudentHavingCourse(int StudentId)
        {
            return _Context.StudentCourses.Where(sc => sc.StudentId == StudentId).FirstOrDefault();
        }

        public bool AddStudentCourse(StudentCourse studentCourse)
        {
            _Context.StudentCourses.Add(studentCourse);
            return _Context.SaveChanges() > 0;
        }

        public bool UpdateStudentCourse(StudentCourse studentCourse)
        {
            _Context.StudentCourses.Update(studentCourse);
            return _Context.SaveChanges() > 0;
        }

        public bool DeleteStudentCourse(StudentCourse studentCourse)
        {
            _Context.StudentCourses.Remove(studentCourse);
            return _Context.SaveChanges() > 0;
        }
    }
}
