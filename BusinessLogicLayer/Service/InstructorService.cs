using DataAccessLayer.Context;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Service
{
    public class InstructorService
    {
        public readonly UniversityContext _context;
        public InstructorService(UniversityContext context)
        {
            _context = context;
        }

        public List<Instructor> GetAllInstructors() => _context.Instructors.ToList();

        public Instructor? GetInstructorById(int Id) => _context.Instructors.FirstOrDefault(s => s.Id == Id);

        public bool AddInstructor(Instructor Instructor)
        {
            _context.Instructors.Add(Instructor);
            return _context.SaveChanges() > 0;
        }

        public bool UpdateInstructor(Instructor Instructor)
        {

            _context.Instructors.Update(Instructor);
            if (_context.SaveChanges() > 0)
            {
                return true;
            }
            else
                return false;
        }
    }
}
