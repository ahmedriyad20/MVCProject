using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entity
{
    public class Student
    {
        [Key]
        public int SSN { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        public int Age { get; set; }
        [StringLength(30)]
        public string? Address { get; set; }
        [StringLength(100)]
        public string? ImageURL { get; set; }
        [MaxLength(30)]
        public string? Email { get; set; }
    }
}
