using BusinessLogicLayer.IService;
using BusinessLogicLayer.Service;
using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.ViewModel;
using Microsoft.AspNetCore.Mvc;
using MVCProject.Filters;

namespace MVCProject.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _DepartmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _DepartmentService = departmentService;
        }

        [Route("Departments/All")]
        public IActionResult GetAll()
        {
            List<Department> Departments = _DepartmentService.GetAllDepartments();

            return View("GetAll", Departments);
        }

        [Route("Departments/{id:int:min(1)}")]
        public IActionResult GetById(int Id)
        {
            DepartmentBranch deptBranch = _DepartmentService.GetDepartmentBranchById(Id);
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

        //Route Configuration is in Program.cs
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
            if(ModelState.IsValid)
            {
                if (_DepartmentService.AddDepartment(department))
                {
                    return RedirectToAction("GetAll");
                }
            }

            return View("Add", department);
        }

        [HttpGet]
        public IActionResult AddV2()
        {
            return View();
        }

        [HttpPost]
        [DepartmentLocationActionFilter]
        public IActionResult AddV2(DepartmentBranch department)
        {
            if (ModelState.IsValid)
            {
                if (_DepartmentService.AddDepartment(department))
                {
                    return RedirectToAction("GetAll");
                }
            }

            return View("AddV2", department);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            DepartmentBranch Department = _DepartmentService.GetDepartmentBranchById(Id);
            return View("Edit", Department);
        }

        [HttpPost]
        public IActionResult Edit(DepartmentBranch department)
        {
            if(ModelState.IsValid)
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
            _DepartmentService.DeleteAllDepartmentBranches(Id);
            //if (!_DepartmentService.DeleteAllDepartmentBranches(Id))
            //    return RedirectToAction("GetAll");

            Department? department = _DepartmentService.GetDepartmentById(Id);

            if (department != null)
            {
                if (_DepartmentService.DeleteDepartment(department))
                {
                    return RedirectToAction("GetAll");
                }
            }

            return View("GetAll");
        }
    }
}
