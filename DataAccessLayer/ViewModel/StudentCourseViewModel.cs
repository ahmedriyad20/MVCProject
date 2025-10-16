using DataAccessLayer.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.ViewModel
{
    public class StudentCourseViewModel
    {
        public int SSN { get; set; }

        [StringLength(30)]
        [Display(Name = "Full Name")] // will only affect the label tag in the view not the database column name
        [RegularExpression("[a-zA-Z ]{3,30}", ErrorMessage = "Only letters and spaces are allowed.")]
        public string Name { get; set; } = null!;

        [Range(17, 35, ErrorMessage = "Age must be between 17 and 35")]
        public int Age { get; set; }

        [StringLength(30)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? ImageURL { get; set; }

        [StringLength(30)]
        [DataType(DataType.EmailAddress)]
        [UniqueEmail]
        public string? Email { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }

        public int? CourseId { get; set; }

        [Range(0, 100, ErrorMessage = "Grade is between 0 and 100")]
        [Required(ErrorMessage = "Grade is required")]
        public float? Grade { get; set; }
        //public string CourseName { get; set; }
    }
}
