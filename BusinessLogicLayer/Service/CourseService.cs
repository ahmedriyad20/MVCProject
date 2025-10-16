using DataAccessLayer.Context;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Service
{
    public class CourseService
    {
        public readonly UniversityContext _context;
        public CourseService(UniversityContext context)
        {
            _context = context;
        }

        public List<Course> GetAllCourses() => _context.Courses.ToList();

        public Course? GetCourseById(int Id) => _context.Courses.FirstOrDefault(s => s.Id == Id);

        public bool AddCourse(Course Course)
        {
            _context.Courses.Add(Course);
            return _context.SaveChanges() > 0;
        }

        public bool UpdateCourse(Course Course)
        {

            _context.Courses.Update(Course);
            if (_context.SaveChanges() > 0)
            {
                return true;
            }
            else
                return false;
        }
    }
}
