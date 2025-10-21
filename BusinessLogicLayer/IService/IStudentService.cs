using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.IService
{
    public interface IStudentService
    {
        public List<Student> GetAllStudents();

        public List<StudentCourse> GetAllStudentCourses(int studentId);

        public Student? GetStudentById(int SSN);

        public bool AddStudent(Student student);

        public bool UpdateStudent(Student student);

        public bool DeleteStudent(Student student);


        public StudentCourse GetStudentCourse(int StudentId);

        public StudentCourse GetStudentHavingCourse(int StudentId);

        public bool AddStudentCourse(StudentCourse studentCourse);

        public bool UpdateStudentCourse(StudentCourse studentCourse);

        public bool DeleteStudentCourse(StudentCourse studentCourse);
    }
}
