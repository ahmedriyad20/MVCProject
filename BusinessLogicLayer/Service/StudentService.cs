using DataAccessLayer.Context;
using DataAccessLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Service
{
    public class StudentService
    {
        static StudentContext Context = new StudentContext();
        public static List<Student> GetAllStudents()
        {
            List<Student> students = Context.Students.ToList();

            return students;
        }

        public static Student GetStudentById(int SSN)
        {
            Student? student = Context.Students.FirstOrDefault(s => s.SSN == SSN);

            return student;
        }
    }
}
