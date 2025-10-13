using DataAccessLayer.Context;
using DataAccessLayer.Models;
using DataAccessLayer.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Service
{
    public class DepartmentService
    {
        static UniversityContext Context = new UniversityContext();
        public static List<Department> GetAllDepartments()
        {
            List<Department> Departments = Context.Departments.Include(d => d.Branches).ToList();

            return Departments;
        }

        public static Department GetDepartmentById(int Id)
        {
            Department? Department = Context.Departments.Include(d => d.Branches).FirstOrDefault(d => d.Id == Id);

            return Department;
        }

        public static Department GetDepartmentByName(string Name)
        {
            Department? Department = Context.Departments.Include(d => d.Branches).FirstOrDefault(d => d.Name == Name);

            return Department;
        }

        public static bool AddDepartment(DepartmentBranch department)
        {
            Department dept = new Department()
            {
                Name = department.Name,
                ManagerName = department.ManagerName,
                Location = department.Location
            };

            Context.Departments.Add(dept);

            if(Context.SaveChanges() > 0)
            {
                Branch branch = new Branch()
                {
                    BranchName = department.BranchName,
                    DepartmentId = dept.Id
                };
                Context.Branches.Add(branch);

                if(Context.SaveChanges() > 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
