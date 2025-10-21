using BusinessLogicLayer.IService;
using BusinessLogicLayer.Service;
using DataAccessLayer.Context;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace MVCProject.Controllers
{
    public class CourseController : Controller
    {
        private ICourseService _CourseService;

        public CourseController(ICourseService courseService)
        {
            _CourseService = courseService;
        }

        [Route("Courses/All")]
        [Route("Courses")]
        public IActionResult GetAll()
        {
            var Courses = _CourseService.GetAllCourses();
            return View("GetAll", Courses);
        }

        [Route("Courses/{id:int:min(1)}")]
        public IActionResult GetById(int Id)
        {
            var Course = _CourseService.GetCourseById(Id);
            ViewBag.Students = _CourseService.GetAllStudentsByCourseId(Id);
            ViewBag.Instructors = _CourseService.GetAllInstructorsByCourseId(Id);
            return View("GetById", Course);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View("Add");
        }

        [HttpPost]
        public IActionResult Add(Course Course)
        {
            if (ModelState.IsValid)
            {
                if (_CourseService.AddCourse(Course))
                {
                    return RedirectToAction("GetAll");
                }
            }

            return View("Add", Course);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var Course = _CourseService.GetCourseById(Id);
            return View("Edit", Course);
        }

        [HttpPost]
        public IActionResult Edit(Course Course)
        {
            if (ModelState.IsValid)
            {
                if (_CourseService.UpdateCourse(Course))
                {
                    return RedirectToAction("GetAll");
                }
            }

            return View("Edit", Course);
        }

        public IActionResult Delete(int Id)
        {
            Course? Course = _CourseService.GetCourseById(Id);

            if (Course != null)
            {
                if (_CourseService.DeleteCourse(Course))
                {
                    return RedirectToAction("GetAll");
                }
            }

            return View("GetAll");
        }
    }
}
