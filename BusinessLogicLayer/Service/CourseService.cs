using BusinessLogicLayer.IService;
using DataAccessLayer.Context;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Service
{
    public class CourseService : ICourseService
    {
        public readonly UniversityContext _Context;
        public CourseService(UniversityContext context)
        {
            _Context = context;
        }

        public List<Course> GetAllCourses() => _Context.Courses.ToList();

        public Course? GetCourseById(int Id) => _Context.Courses.FirstOrDefault(s => s.Id == Id);

        public bool AddCourse(Course Course)
        {
            _Context.Courses.Add(Course);
            return _Context.SaveChanges() > 0;
        }

        public bool UpdateCourse(Course Course)
        {

            _Context.Courses.Update(Course);
            if (_Context.SaveChanges() > 0)
            {
                return true;
            }
            else
                return false;
        }

        public bool DeleteCourse(Course Course)
        {
            _Context.Courses.Remove(Course);
            return _Context.SaveChanges() > 0;
        }

        ////////////////////////////////////////// Student,Instructor-Course Management Section //////////////////////////////////////////

        public List<Student> GetAllStudentsByCourseId(int CourseId)
        {
            return _Context.Students.Where(s => s.StudentCourses.Any(sc => sc.CourseId == CourseId)).ToList();
        }

        public List<Instructor> GetAllInstructorsByCourseId(int CourseId)
        {
            return _Context.Instructors.Where(i => i.InstructorCourses.Any(ic => ic.CourseId == CourseId)).ToList();
        }
    }
}
