using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Validators
{
    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var studentObj = validationContext.ObjectInstance as StudentCourseViewModel;
            var ssnProperty = validationContext.ObjectInstance.GetType().GetProperty("SSN");

            // to avoid error when update student without change email
            if (studentObj?.SSN == 0)
            {
                string Email = value as string;

                UniversityContext context = new UniversityContext();
                var student = context.Students.FirstOrDefault(s => s.Email == Email);
                if (student != null)
                {
                    return new ValidationResult("Email Already Exist");
                }
            }
             

            return ValidationResult.Success;
        }
    }
}
