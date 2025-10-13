using BusinessLogicLayer.Service;
using DataAccessLayer.Models;
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

        public IActionResult Add()
        {
            return View("Add");
        }

        public IActionResult AddStudent(Student student)
        {
            bool isAdded = StudentService.AddStudent(student);
            if (isAdded)
            {
                return RedirectToAction("GetAll");
            }

            return View("Add");
        }
    }
}
