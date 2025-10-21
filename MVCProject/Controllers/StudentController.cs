using BusinessLogicLayer.IService;
using BusinessLogicLayer.Service;
using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MVCProject.Controllers
{
    public class StudentController : Controller
    {
        //Prepare the Service Layer Object
        private IStudentService _StudentService;
        private IDepartmentService _DepartmentService;
        private ICourseService _CourseService;

        public StudentController(IStudentService studentService, IDepartmentService departmentService, ICourseService courseService)
        {
            _StudentService = studentService;
            _DepartmentService = departmentService;
            _CourseService = courseService;
        }

        [Route("Students/All")]
        [Route("Students")]
        public IActionResult GetAll()
        {
            List<Student> students = _StudentService.GetAllStudents();
            return View("GetAll", students);
        }

        [Route("Students/{Id}")]
        public IActionResult GetById(int Id)
        {
            Student? student = _StudentService.GetStudentById(Id);
            ViewBag.StudentCourses = _StudentService.GetAllStudentCourses(Id);
            return View("GetById", student);
        }

        [HttpGet]
        public IActionResult Add()
        {
            List<Department> departments = _DepartmentService.GetAllDepartments();
            ViewBag.Departments = departments;
            ViewBag.Courses = _CourseService.GetAllCourses();
            return View("Add");
        }

        [HttpPost]
        public IActionResult Add(StudentCourseViewModel studentCourse)
        {
            // Validate DepartmentId and CourseId, Make sure they are not zero
            if (studentCourse.DepartmentId == 0)
            {
                ModelState.AddModelError("DepartmentId", "The Department field is required.");
            }

            if(studentCourse.CourseId == 0)
            {
                ModelState.AddModelError("CourseId", "The Course field is required.");
            }

            if (ModelState.IsValid)
            {
                Student student = new Student()
                {
                    Name = studentCourse.Name,
                    Age = studentCourse.Age,
                    Address = studentCourse.Address,
                    ImageURL = studentCourse.ImageURL,
                    Email = studentCourse.Email,
                    DepartmentId = studentCourse.DepartmentId
                };

                bool isAdded = _StudentService.AddStudent(student);
                if (isAdded)
                {
                    // Now add the student course
                    StudentCourse sc = new StudentCourse()
                    {
                        StudentId = student.SSN,
                        CourseId = (int)studentCourse.CourseId,
                        Grade = (float)studentCourse.Grade
                    };
            
                    if (_StudentService.AddStudentCourse(sc))
                        //Student Course added successfully
                        return RedirectToAction("GetAll");
                }
            
            }

            // If we reach here, something went wrong, redisplay the form
            ViewBag.Departments = _DepartmentService.GetAllDepartments();
            ViewBag.Courses = _CourseService.GetAllCourses();
            return View("Add", studentCourse);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            Student? student = _StudentService.GetStudentById(Id);

            StudentCourseViewModel studentCourse = new StudentCourseViewModel()
            {
                SSN = student.SSN,
                Name = student.Name,
                Age = student.Age,
                Address = student.Address,
                ImageURL = student.ImageURL,
                Email = student.Email,
                DepartmentId = student.DepartmentId,
                CourseId = _StudentService.GetStudentCourse(Id).CourseId,
                Grade = _StudentService.GetStudentCourse(Id).Grade
            };

            ViewBag.Departments = _DepartmentService.GetAllDepartments(); 
            ViewBag.Courses = _CourseService.GetAllCourses();
            return View("Edit", studentCourse);
        }

        [HttpPost]
        public IActionResult Edit(StudentCourseViewModel studentCourse)
        {
            // Validate DepartmentId and CourseId, Make sure they are not zero
            if (studentCourse.DepartmentId == 0)
            {
                ModelState.AddModelError("DepartmentId", "The Department field is required.");
            }

            if (studentCourse.CourseId == 0)
            {
                ModelState.AddModelError("CourseId", "The Course field is required.");
            }

            if (ModelState.IsValid)
            {
                Student student = new Student()
                {
                    SSN = studentCourse.SSN,
                    Name = studentCourse.Name,
                    Age = studentCourse.Age,
                    Address = studentCourse.Address,
                    ImageURL = studentCourse.ImageURL,
                    Email = studentCourse.Email,
                    DepartmentId = studentCourse.DepartmentId
                };

                if (_StudentService.UpdateStudent(student))
                {
                    StudentCourse sc = _StudentService.GetStudentHavingCourse(student.SSN);
                    //StudentCourse sc = _Context.StudentCourses.Where(sc => sc.StudentId == student.SSN).FirstOrDefault();
                    // Now Update the student course if exists

                    #region Very Improtant Note
                    //if you want to update a record in EF and this record consists of composite key you have to remove
                    //it from the database first then add it again with the updated values
                    #endregion

                    if (sc != null)
                    {
                        _StudentService.DeleteStudentCourse(sc);

                        StudentCourse newSC = new StudentCourse()
                        {
                            StudentId = student.SSN,
                            CourseId = (int)studentCourse.CourseId,
                            Grade = (float)studentCourse.Grade
                        };

                        if (_StudentService.AddStudentCourse(newSC))
                            //Student Course updated successfully
                            return RedirectToAction("GetAll");
                    }
                    else
                    {
                        // if not exists Add new student course record
                        StudentCourse newSC = new StudentCourse()
                        {
                            StudentId = student.SSN,
                            CourseId = (int)studentCourse.CourseId,
                            Grade = (float)studentCourse.Grade
                        };

                        if (_StudentService.AddStudentCourse(newSC))
                            //Student Course updated successfully
                            return RedirectToAction("GetAll");
                    }
                }

            }

            // If we reach here, something went wrong, redisplay the form
            ViewBag.Departments = _DepartmentService.GetAllDepartments();
            ViewBag.Courses = _CourseService.GetAllCourses();
            return View("Edit", studentCourse);

        }

        public IActionResult Delete(int Id)
        {
            Student? student = _StudentService.GetStudentById(Id);

            if(student != null)
            {
                if(_StudentService.DeleteStudent(student))
                {
                    return RedirectToAction("GetAll");
                }
            }

            return View("GetAll");
        }
    }
}
