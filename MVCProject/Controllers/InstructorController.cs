using BusinessLogicLayer.Service;
using DataAccessLayer.Context;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace MVCProject.Controllers
{
    public class InstructorController : Controller
    {
        private readonly UniversityContext _Context;
        private readonly InstructorService _InstructorService;
        public InstructorController()
        {
            _Context = new UniversityContext();
            _InstructorService = new InstructorService(_Context);
        }

        public IActionResult GetAll()
        {
            var Instructors = _InstructorService.GetAllInstructors();
            return View("GetAll", Instructors);
        }

        public IActionResult GetById(int Id)
        {
            var Instructor = _InstructorService.GetInstructorById(Id);
            ViewBag.deptName = _Context.Departments.FirstOrDefault(d => d.Id == Instructor.DepartmentId)?.Name;
            return View("GetById", Instructor);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Departments = _Context.Departments.ToList();
            return View("Add");
        }

        [HttpPost]
        public IActionResult Add(Instructor instructor)
        {
            if(instructor.Name != null)
            {
                if(_InstructorService.AddInstructor(instructor))
                {
                    return RedirectToAction("GetAll");
                }
            }

            ViewBag.Departments = _Context.Departments.ToList();
            return View("Add", instructor);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var instructor = _InstructorService.GetInstructorById(Id);
            ViewBag.Departments = _Context.Departments.ToList();
            return View("Edit", instructor);
        }

        [HttpPost]
        public IActionResult Edit(Instructor instructor)
        {
            if(instructor.Name != null)
            {
                if(_InstructorService.UpdateInstructor(instructor))
                {
                    return RedirectToAction("GetAll");
                }
            }

            ViewBag.Departments = _Context.Departments.ToList();
            return View("Edit", instructor);
        }

        public IActionResult Delete(int Id)
        {
            Instructor? Instructor = _InstructorService.GetInstructorById(Id);

            if (Instructor != null)
            {
                _Context.Instructors.Remove(Instructor);
                if (_Context.SaveChanges() > 0)
                {
                    return RedirectToAction("GetAll");
                }
            }

            return View("GetAll");
        }
    }
}
