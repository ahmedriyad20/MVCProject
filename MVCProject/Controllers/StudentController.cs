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
        private readonly UniversityContext _Context;
        public StudentController()
        {
            _Context = new UniversityContext();
        }
        public IActionResult GetAll()
        {
            StudentService studentService = new StudentService(_Context);
            List<Student> students = studentService.GetAllStudents();
            //ViewBag.Courses = _Context.Courses.Where(c => c.StudentCourses.Any(sc => students.Any(s => s.SSN == sc.StudentId))).ToList();
            return View("GetAll", students);
        }

        public IActionResult GetById(int Id)
        {
            StudentService studentService = new StudentService(_Context);
            Student? student = studentService.GetStudentById(Id);
            //ViewBag.Courses = _Context.Courses.Where(c => c.StudentCourses.Any(sc => sc.StudentId == Id)).ToList();
            ViewBag.StudentCourses = _Context.StudentCourses.Include(sc => sc.Course).Where(sc => sc.StudentId == Id).ToList();
            return View("GetById", student);
        }

        [HttpGet]
        public IActionResult Add()
        {
            DepartmentService departmentService = new DepartmentService(_Context);
            List<Department> departments = departmentService.GetAllDepartments();
            ViewBag.Departments = departments;
            ViewBag.Courses = _Context.Courses.ToList();
            return View("Add");
        }

        [HttpPost]
        public IActionResult Add(StudentCourseViewModel studentCourse)
        {
            if(studentCourse.DepartmentId != 0)
            {
                if (ModelState.IsValid)
                {
                    StudentService studentService = new StudentService(_Context);
                    Student student = new Student()
                    {
                        Name = studentCourse.Name,
                        Age = studentCourse.Age,
                        Address = studentCourse.Address,
                        ImageURL = studentCourse.ImageURL,
                        Email = studentCourse.Email,
                        DepartmentId = studentCourse.DepartmentId
                    };
                    bool isAdded = studentService.AddStudent(student);
                    if (isAdded)
                    {
                        // Now add the student course
                        StudentCourse sc = new StudentCourse()
                        {
                            StudentId = student.SSN,
                            CourseId = (int)studentCourse.CourseId,
                            Grade = (float)studentCourse.Grade
                        };
                        _Context.StudentCourses.Add(sc);

                        if (_Context.SaveChanges() > 0)
                            //Student Course added successfully
                            return RedirectToAction("GetAll");
                    }

                }
            }
            else
            {
                ModelState.AddModelError("DepartmentId", "The Department field is required.");
            }


            DepartmentService departmentService = new DepartmentService(_Context);
            List<Department> departments = departmentService.GetAllDepartments();
            ViewBag.Departments = departments;
            ViewBag.Courses = _Context.Courses.ToList();
            return View("Add", studentCourse);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            StudentService studentService = new StudentService(_Context);
            Student? student = studentService.GetStudentById(Id);

            StudentCourseViewModel studentCourse = new StudentCourseViewModel()
            {
                SSN = student.SSN,
                Name = student.Name,
                Age = student.Age,
                Address = student.Address,
                ImageURL = student.ImageURL,
                Email = student.Email,
                DepartmentId = student.DepartmentId,
                CourseId = _Context.StudentCourses.FirstOrDefault(sc => sc.StudentId == Id)?.CourseId,
                Grade = (float)(_Context.StudentCourses.FirstOrDefault(sc => sc.StudentId == Id)?.Grade ?? 0)
            };

            DepartmentService departmentService = new DepartmentService(_Context);
            List<Department> departments = departmentService.GetAllDepartments();
            ViewBag.Departments = departments;
            ViewBag.Courses = _Context.Courses.ToList();
            return View("Edit", studentCourse);
        }

        [HttpPost]
        public IActionResult Edit(StudentCourseViewModel studentCourse)
        {
            if(studentCourse.DepartmentId != 0)
            {
                if (ModelState.IsValid)
                {
                    StudentService studentService = new StudentService(_Context);
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

                    if (studentService.UpdateStudent(student))
                    {
                        StudentCourse sc = _Context.StudentCourses.Where(sc => sc.StudentId == student.SSN).FirstOrDefault();
                        // Now Update the student course if exists

                        #region Very Improtant Note
                        //if you want to update a record in EF and this record consists of composite key you have to remove
                        //it from the database first then add it again with the updated values
                        #endregion

                        if (sc != null)
                        {

                            _Context.StudentCourses.Remove(sc);
                            _Context.SaveChanges();

                            StudentCourse newSC = new StudentCourse()
                            {
                                StudentId = student.SSN,
                                CourseId = (int)studentCourse.CourseId,
                                Grade = (float)studentCourse.Grade
                            };
                            
                            _Context.StudentCourses.Add(newSC);

                            if (_Context.SaveChanges() > 0)
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
                            _Context.StudentCourses.Add(newSC);

                            if (_Context.SaveChanges() > 0)
                                //Student Course added successfully
                                return RedirectToAction("GetAll");
                        }
                        
                    }

                }
            }
            else
            {
                ModelState.AddModelError("DepartmentId", "The Department field is required.");
            }


            DepartmentService departmentService = new DepartmentService(_Context);
            List<Department> departments = departmentService.GetAllDepartments();
            ViewBag.Departments = departments;
            ViewBag.Courses = _Context.Courses.ToList();
            return View("Edit", studentCourse);

        }

        public IActionResult Delete(int Id)
        {
            StudentService studentService = new StudentService(_Context);
            Student? student = studentService.GetStudentById(Id);

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
