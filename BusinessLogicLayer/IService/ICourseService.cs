using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.IService
{
    public interface ICourseService
    {
        public List<Course> GetAllCourses();

        public Course? GetCourseById(int Id);

        public bool AddCourse(Course Course);

        public bool UpdateCourse(Course Course);

        public bool DeleteCourse(Course Course);

        ////////////////////////////////////////// Student,Instructor-Course Management Section //////////////////////////////////////////

        public List<Student> GetAllStudentsByCourseId(int CourseId);

        public List<Instructor> GetAllInstructorsByCourseId(int CourseId);
    }
}
