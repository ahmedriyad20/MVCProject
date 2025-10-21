using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.IService
{
    public interface IInstructorService
    {
        public List<Instructor> GetAllInstructors();

        public Instructor? GetInstructorById(int Id);

        public bool AddInstructor(Instructor Instructor);

        public bool UpdateInstructor(Instructor Instructor);

        public bool DeleteInstructor(int Id);

        ////////////////////////////////////////// Instructor-Course Management Section //////////////////////////////////////////

        public List<InstructorCourse> GetAllInstructorCourses(int InstructorId);

        public InstructorCourse GetInstructorCourse(int InstructorId, int CourseId);

        public bool AddInstructorCourse(InstructorCourse instructorCourse);

        public bool UpdateInstructorCourse(InstructorCourse instructorCourse);

        public bool DeleteInstructorCourse(InstructorCourse InstructorCourse);
    }
}
