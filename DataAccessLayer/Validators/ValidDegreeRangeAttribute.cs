using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Validators
{
    public class ValidDegreeRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            Course? course = validationContext.ObjectInstance as Course;

            int Degree = Convert.ToInt32(value);

            if(course?.MinDegree >= Degree)
            {
                return new ValidationResult("Degree must be greater than Min Degree.");
            }

            return ValidationResult.Success;
        }
    }
}
