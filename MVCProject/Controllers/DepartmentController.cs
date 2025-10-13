using BusinessLogicLayer.Service;
using DataAccessLayer.Models;
using DataAccessLayer.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace MVCProject.Controllers
{
    public class DepartmentController : Controller
    {
        public IActionResult GetAll()
        {
            List<Department> Departments = DepartmentService.GetAllDepartments();

            return View("GetAll", Departments);
        }

        public IActionResult GetById(int Id)
        {
            Department Department = DepartmentService.GetDepartmentById(Id);

            return View("GetById", Department);
        }

        public IActionResult GetByName(string Name)
        {
            Department Department = DepartmentService.GetDepartmentByName(Name);

            return View("GetByName", Department);
        }

        public IActionResult Add()
        {
            return View("Add");
        }

        public IActionResult AddDepartment(DepartmentBranch department)
        {
            bool isAdded = DepartmentService.AddDepartment(department);
            if (isAdded)
            {
                return RedirectToAction("GetAll");
            }

            return View("Add");
        }
    }
}
