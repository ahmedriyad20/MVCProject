using BusinessLogicLayer.Service;
using DataAccessLayer.Context;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            ViewBag.Courses = _Context.InstructorCourses.Include(ic => ic.Course).Where(ic => ic.InstructorId == Id).ToList();
            return View("GetById", Instructor);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Departments = _Context.Departments.ToList();
            return View("Add2");
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

        public IActionResult ManageCourses(int Id)
        {
            var instructor = _InstructorService.GetInstructorById(Id);
            ViewBag.Courses = _Context.InstructorCourses.Include(ic => ic.Course).Where(ic => ic.InstructorId == Id).ToList();
            return View(instructor);
        }

        [HttpGet]
        public IActionResult AddCourse(int Id)
        {
            ViewBag.Courses = _Context.Courses.ToList();
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
                _Context.InstructorCourses.Add(instructorCourse);
                if (_Context.SaveChanges() > 0)
                {
                    return RedirectToAction("ManageCourses", new { Id = instructorCourse.InstructorId });
                }
                    //return RedirectToAction("ManageCourses");
            }

            ViewBag.Courses = _Context.Courses.ToList();
            return View("AddCourse", instructorCourse);
        }

        [HttpGet]
        public IActionResult UpdateCourse(int InstructorId, int CourseId)
        {
            InstructorCourse? instructorCourse = _Context.InstructorCourses.FirstOrDefault(ic => ic.InstructorId == InstructorId && ic.CourseId == CourseId);
            ViewBag.Courses = _Context.Courses.ToList();
            return View(instructorCourse);
        }

        [HttpPost]
        public IActionResult UpdateCourse(InstructorCourse instructorCourse)
        {
            if (ModelState.IsValid)
            {
                _Context.InstructorCourses.Update(instructorCourse);
                if (_Context.SaveChanges() > 0)
                    return RedirectToAction("ManageCourses", new { Id = instructorCourse.InstructorId });
            }

            ViewBag.Courses = _Context.Courses.ToList();
            return View("UpdateCourse", instructorCourse);
        }

        public IActionResult DeleteCourse(int InstructorId, int CourseId)
        {
            InstructorCourse? instructorCourse = _Context.InstructorCourses.FirstOrDefault(ic => ic.InstructorId == InstructorId && ic.CourseId == CourseId);
            if (instructorCourse != null)
            {
                _Context.InstructorCourses.Remove(instructorCourse);
                if (_Context.SaveChanges() > 0)
                    return RedirectToAction("ManageCourses", new { Id = InstructorId });
            }
            return View("ManageCourses");
        }
    }
}
