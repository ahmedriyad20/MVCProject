using BusinessLogicLayer.Service;
using DataAccessLayer.Entity;
using Microsoft.AspNetCore.Mvc;

namespace MVCProject.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult GetAll()
        {
            List<Student> students = StudentService.GetAllStudents();

            return View("GetAll", students);
        }

        public IActionResult GetById(int SSN)
        {
            Student student = StudentService.GetStudentById(SSN);

            return View("GetById", student);
        }
    }
}
