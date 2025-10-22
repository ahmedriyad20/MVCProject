using BusinessLogicLayer.IService;
using BusinessLogicLayer.Service;
using DataAccessLayer.Context;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCProject.Filters;

namespace MVCProject.Controllers
{
    [Authorize]
    public class InstructorController : Controller
    {
        // Access to the DbContext
        private IInstructorService _InstructorService;
        private IDepartmentService _DepartmentService;
        private ICourseService _CourseService;


        public InstructorController(IInstructorService instructorService, IDepartmentService departmentService, ICourseService courseService)
        {
            _InstructorService = instructorService;
            _DepartmentService = departmentService;
            _CourseService = courseService;
        }

        [Authorize(Roles = "Admin,Student,Instructor")]
        [Route("Instructors/All")]
        [ExceptionHandleFilter]
        [ResourceFilter("instructors-list", 60)]
        public IActionResult GetAll()
        {
            //throw new Exception("Test exception to verify ExceptionHandleFilter");

            var Instructors = _InstructorService.GetAllInstructors();
            return View("GetAll", Instructors);
        }

        [Authorize(Roles = "Admin,Instructor")]
        [Route("Instructors/{id:int:min(1)}")]
        [AuthorizationFilter(true)]
        public IActionResult GetById(int Id)
        {
            var Instructor = _InstructorService.GetInstructorById(Id);
            ViewBag.deptName = _DepartmentService.GetDepartmentById((int)Instructor.DepartmentId).Name;
            ViewBag.Courses = _InstructorService.GetAllInstructorCourses(Id);
            return View("GetById", Instructor);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Departments = _DepartmentService.GetAllDepartments();
            return View("Add");
        }

        [HttpPost]
        public IActionResult Add(Instructor instructor)
        {
            if (instructor.DepartmentId != 0)
            {
                if (ModelState.IsValid)
                {
                    if (_InstructorService.AddInstructor(instructor))
                    {
                        return RedirectToAction("GetAll");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("DepartmentId", "Please select a Department");
            }


            ViewBag.Departments = _DepartmentService.GetAllDepartments();
            return View("Add", instructor);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var instructor = _InstructorService.GetInstructorById(Id);
            ViewBag.Departments = _DepartmentService.GetAllDepartments();
            return View("Edit", instructor);
        }

        [EditInstructorResultFilter]
        [HttpPost]
        public IActionResult Edit(Instructor instructor)
        {
            if (instructor.DepartmentId == 0)
            {
                ModelState.AddModelError("DepartmentId", "Please select a Department");
            }
            if (ModelState.IsValid)
            {
                if (_InstructorService.UpdateInstructor(instructor))
                {
                    return RedirectToAction("GetAll");
                }
            }

            ViewBag.Departments = _DepartmentService.GetAllDepartments();
            return View("Edit", instructor);
        }

        public IActionResult Delete(int Id)
        {
            Instructor? Instructor = _InstructorService.GetInstructorById(Id);

            if (Instructor != null)
            {
                if (_InstructorService.DeleteInstructor(Id))
                {
                    return RedirectToAction("GetAll");
                }
            }

            return View("GetAll");
        }

        ////////////////////////////////////////// Instructor-Course Management Section //////////////////////////////////////////

        [Authorize(Roles = "Admin")]
        public IActionResult ManageCourses(int Id)
        {
            var instructor = _InstructorService.GetInstructorById(Id);
            ViewBag.Courses = _InstructorService.GetAllInstructorCourses(Id);
            return View(instructor);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddCourse(int Id)
        {
            ViewBag.Courses = _CourseService.GetAllCourses();
            ViewBag.InstructorId = Id;
            InstructorCourse instructorCourse = new InstructorCourse();
            instructorCourse.InstructorId = Id;
            return View(instructorCourse);
        }

        [HttpPost]
        public IActionResult AddCourse(InstructorCourse instructorCourse)
        {
            if (instructorCourse.CourseId == 0)
            {
                ModelState.AddModelError("CourseId", "Please select a Course");
            }

            if (ModelState.IsValid)
            {
                if (_InstructorService.AddInstructorCourse(instructorCourse))
                {
                    return RedirectToAction("ManageCourses", new { Id = instructorCourse.InstructorId });
                }
                    //return RedirectToAction("ManageCourses");
            }

            ViewBag.Courses = _CourseService.GetAllCourses();
            return View("AddCourse", instructorCourse);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult UpdateCourse(int InstructorId, int CourseId)
        {
            InstructorCourse? instructorCourse = _InstructorService.GetInstructorCourse(InstructorId, CourseId);
            ViewBag.Courses = _CourseService.GetAllCourses();
            return View(instructorCourse);
        }

        [HttpPost]
        public IActionResult UpdateCourse(InstructorCourse instructorCourse)
        {
            if (ModelState.IsValid)
            {
                if (_InstructorService.UpdateInstructorCourse(instructorCourse))
                    return RedirectToAction("ManageCourses", new { Id = instructorCourse.InstructorId });
            }

            ViewBag.Courses = _CourseService.GetAllCourses();
            return View("UpdateCourse", instructorCourse);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCourse(int InstructorId, int CourseId)
        {
            InstructorCourse? instructorCourse = _InstructorService.GetInstructorCourse(InstructorId, CourseId);
            if (instructorCourse != null)
            {
                if (_InstructorService.DeleteInstructorCourse(instructorCourse))
                    return RedirectToAction("ManageCourses", new { Id = InstructorId });
            }
            return View("ManageCourses");
        }
    }
}
