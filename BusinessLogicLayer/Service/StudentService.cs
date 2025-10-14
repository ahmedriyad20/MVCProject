using DataAccessLayer.Context;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Service
{
    public class StudentService
    {
        private readonly UniversityContext _context;
        public StudentService(UniversityContext context)
        {
            _context = context; 
        }

        public List<Student> GetAllStudents() => _context.Students.ToList();

        public Student? GetStudentById(int SSN) => _context.Students.FirstOrDefault(s => s.SSN == SSN);

        public bool AddStudent(Student student)
        {
            _context.Students.Add(student);
            return _context.SaveChanges() > 0;
        }

        public bool UpdateStudent(Student student)
        {

            _context.Students.Update(student);
            if (_context.SaveChanges() > 0)
            {
                return true;
            }
            else
                return false;
        }
    }
}
