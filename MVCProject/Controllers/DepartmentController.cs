using BusinessLogicLayer.Service;
using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace MVCProject.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly UniversityContext _Context;
        private readonly DepartmentService _DepartmentService;
        public DepartmentController()
        {
            _Context = new UniversityContext();
            _DepartmentService = new DepartmentService(_Context);
        }
        public IActionResult GetAll()
        {
            List<Department> Departments = _DepartmentService.GetAllDepartments();

            return View("GetAll", Departments);
        }

        public IActionResult GetById(int Id)
        {
            DepartmentBranch deptBranch = _DepartmentService.GetDepartmentById(Id);
            Department Department = new Department()
            {
                Id = deptBranch.DepartmentId,
                Name = deptBranch.Name,
                ManagerName = deptBranch.ManagerName,
                Location = deptBranch.Location,
                Branches = _DepartmentService.GetAllDepartmentBranches(deptBranch.DepartmentId)
            };

            return View("GetById", Department);
        }

        public IActionResult GetByName(string Name)
        {
            Department Department = _DepartmentService.GetDepartmentByName(Name);

            return View("GetByName", Department);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View("Add");
        }

        [HttpPost]
        public IActionResult AddDepartment(DepartmentBranch department)
        {
            if(department.Name != null)
            {
                if (_DepartmentService.AddDepartment(department))
                {
                    return RedirectToAction("GetAll");
                }
            }

            return View("Add", department);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            DepartmentBranch Department = _DepartmentService.GetDepartmentById(Id);
            return View("Edit", Department);
        }

        [HttpPost]
        public IActionResult Edit(DepartmentBranch department)
        {
            if(department.Name != null)
            {
                if(_DepartmentService.UpdateDepartment(department))
                {
                    return RedirectToAction("GetAll");
                }
            }

            return View("Edit", department);
        }

        public IActionResult Delete(int Id)
        {
            List<Branch> branches = _DepartmentService.GetAllDepartmentBranches(Id);

            foreach(var branch in branches)
            {
                _Context.Branches.Remove(branch);
            }

            if(_Context.SaveChanges() <= 0)
                return RedirectToAction("GetAll");

            Department? department = _Context.Departments.FirstOrDefault(d => d.Id == Id);

            if (department != null)
            {
                _Context.Departments.Remove(department);
                if (_Context.SaveChanges() > 0)
                {
                    return RedirectToAction("GetAll");
                }
            }

            return View("GetAll");
        }
    }
}
