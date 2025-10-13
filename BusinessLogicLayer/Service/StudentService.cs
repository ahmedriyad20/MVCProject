using DataAccessLayer.Context;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Service
{
    public class StudentService
    {
        static UniversityContext Context = new UniversityContext();
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

        public static bool AddStudent(Student student)
        {
            Context.Students.Add(student);
            int affectedRows = Context.SaveChanges();
            return affectedRows > 0;
        }
    }
}
