using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.ViewModel
{
    public class DepartmentBranch
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; } = null!;
        public string? ManagerName { get; set; }
        public string? Location { get; set; }

        public Branches BranchName { get; set; }
    }
}
