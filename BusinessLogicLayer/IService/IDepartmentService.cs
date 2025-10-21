using DataAccessLayer.Models;
using DataAccessLayer.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.IService
{
    public interface IDepartmentService
    {
        public List<Department> GetAllDepartments();

        public List<Branch> GetAllDepartmentBranches(int departmentId);

        public Department GetDepartmentById(int Id);

        public DepartmentBranch GetDepartmentBranchById(int Id);

        public Department GetDepartmentByName(string Name);

        public bool AddDepartment(DepartmentBranch department);

        public bool UpdateDepartment(DepartmentBranch department);

        public bool DeleteDepartment(Department department);

        public bool DeleteAllDepartmentBranches(int DepartmentId);
    }
}
