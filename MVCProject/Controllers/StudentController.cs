using BusinessLogicLayer.Service;
using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace MVCProject.Controllers
{
    public class StudentController : Controller
    {
        private readonly UniversityContext _Context;
        public StudentController()
        {
            _Context = new UniversityContext();
        }
        public IActionResult GetAll()
        {
            StudentService studentService = new StudentService(_Context);
            List<Student> students = studentService.GetAllStudents();

            return View("GetAll", students);
        }

        public IActionResult GetById(int Id)
        {
            StudentService studentService = new StudentService(_Context);
            Student student = studentService.GetStudentById(Id);

            return View("GetById", student);
        }

        [HttpGet]
        public IActionResult Add()
        {
            DepartmentService departmentService = new DepartmentService(_Context);
            List<Department> departments = departmentService.GetAllDepartments();
            ViewBag.Departments = departments;
            return View("Add");
        }

        [HttpPost]
        public IActionResult AddStudent(Student student)
        {
            StudentService studentService = new StudentService(_Context);
            if (student.Name != null)
            {
                bool isAdded = studentService.AddStudent(student);
                if (isAdded)
                {
                    return RedirectToAction("GetAll");
                }
            }
            DepartmentService departmentService = new DepartmentService(_Context);
            List<Department> departments = departmentService.GetAllDepartments();
            ViewBag.Departments = departments;
            return View("Add", student);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            StudentService studentService = new StudentService(_Context);
            Student student = studentService.GetStudentById(Id);

            DepartmentService departmentService = new DepartmentService(_Context);
            List<Department> departments = departmentService.GetAllDepartments();
            ViewBag.Departments = departments;
            return View("Edit", student);
        }

        [HttpPost]
        public IActionResult Edit(Student student)
        {
            StudentService studentService = new StudentService(_Context);

            if (studentService.UpdateStudent(student))
            {
                return RedirectToAction("GetAll");
            }
            else
            {
                DepartmentService departmentService = new DepartmentService(_Context);
                List<Department> departments = departmentService.GetAllDepartments();
                ViewBag.Departments = departments;
                return View("Edit", student);
            }
        }

        public IActionResult Delete(int Id)
        {
            StudentService studentService = new StudentService(_Context);
            Student student = studentService.GetStudentById(Id);

            if(student != null)
            {
                _Context.Students.Remove(student);
                if(_Context.SaveChanges() > 0)
                {
                    return RedirectToAction("GetAll");
                }
            }

            return View("GetAll");
        }
    }
}
