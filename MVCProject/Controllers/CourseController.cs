using BusinessLogicLayer.Service;
using DataAccessLayer.Context;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace MVCProject.Controllers
{
    public class CourseController : Controller
    {
        private readonly UniversityContext _Context;
        private readonly CourseService _CourseService;
        public CourseController()
        {
            _Context = new UniversityContext();
            _CourseService = new CourseService(_Context);
        }

        public IActionResult GetAll()
        {
            var Courses = _CourseService.GetAllCourses();
            return View("GetAll", Courses);
        }

        public IActionResult GetById(int Id)
        {
            var Course = _CourseService.GetCourseById(Id);
            ViewBag.Students = _Context.Students.Where(s => s.StudentCourses.Any(sc => sc.CourseId == Id)).ToList();
            ViewBag.Instructors = _Context.Instructors.Where(i => i.InstructorCourses.Any(ic => ic.CourseId == Id)).ToList();
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
            if (Course.Name != null)
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
            if (Course.Name != null)
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
                _Context.Courses.Remove(Course);
                if (_Context.SaveChanges() > 0)
                {
                    return RedirectToAction("GetAll");
                }
            }

            return View("GetAll");
        }
    }
}
