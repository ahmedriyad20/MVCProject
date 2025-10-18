using DataAccessLayer.Context;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Validators
{
    public class UniqueCourseNameAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            Course? courseObj = validationContext.ObjectInstance as Course;

            string? CourseName = value as string;

            if(courseObj?.Id == 0) //Add Course case
            {
                UniversityContext context = new UniversityContext();

                var course = context.Courses.FirstOrDefault(c => c.Name == CourseName);

                if(course != null)
                {
                    return new ValidationResult("Course Name Already Exist");
                }
            }

            return ValidationResult.Success;
        }
    }
}
