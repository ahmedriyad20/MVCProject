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
    public class InstructorService : IInstructorService
    {
        public readonly UniversityContext _Context;
        public InstructorService(UniversityContext context)
        {
            _Context = context;
        }

        public List<Instructor> GetAllInstructors() => _Context.Instructors.ToList();

        public Instructor? GetInstructorById(int Id) => _Context.Instructors.FirstOrDefault(s => s.Id == Id);

        public bool AddInstructor(Instructor Instructor)
        {
            _Context.Instructors.Add(Instructor);
            return _Context.SaveChanges() > 0;
        }

        public bool UpdateInstructor(Instructor Instructor)
        {

            _Context.Instructors.Update(Instructor);
            if (_Context.SaveChanges() > 0)
            {
                return true;
            }
            else
                return false;
        }

        public bool DeleteInstructor(int Id)
        {
            var instructor = GetInstructorById(Id);
            if (instructor != null)
            {
                _Context.Instructors.Remove(instructor);
                return _Context.SaveChanges() > 0;
            }
            else
                return false;
        }

        ////////////////////////////////////////// Instructor-Course Management Section //////////////////////////////////////////

        public List<InstructorCourse> GetAllInstructorCourses(int InstructorId)
        {
            return _Context.InstructorCourses.Include(ic => ic.Course).Where(ic => ic.InstructorId == InstructorId).ToList();
        }

        public InstructorCourse GetInstructorCourse(int InstructorId, int CourseId)
        {
            return _Context.InstructorCourses.FirstOrDefault(ic => ic.InstructorId == InstructorId && ic.CourseId == CourseId);
        }

        public bool AddInstructorCourse(InstructorCourse instructorCourse)
        {
            _Context.InstructorCourses.Add(instructorCourse);
            return _Context.SaveChanges() > 0;
        }

        public bool UpdateInstructorCourse(InstructorCourse instructorCourse)
        {
            _Context.InstructorCourses.Update(instructorCourse);
            return _Context.SaveChanges() > 0;
        }

        public bool DeleteInstructorCourse(InstructorCourse InstructorCourse)
        {
            var instructorCourse = GetInstructorCourse(InstructorCourse.InstructorId, InstructorCourse.CourseId );
            if (instructorCourse != null)
            {
                _Context.InstructorCourses.Remove(instructorCourse);
                return _Context.SaveChanges() > 0;
            }
            else
                return false;
        }
    }
}
