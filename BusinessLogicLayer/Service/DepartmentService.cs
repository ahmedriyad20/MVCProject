using BusinessLogicLayer.IService;
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
    public class DepartmentService : IDepartmentService
    {
        private readonly UniversityContext _Context;

        public DepartmentService(UniversityContext context)
        {
            _Context = context;
        }

        public  List<Department> GetAllDepartments()
        {
            List<Department> Departments = _Context.Departments.Include(d => d.Branches).ToList();

            return Departments;
        }

        public List<Branch> GetAllDepartmentBranches(int departmentId)
        {
            return _Context.Branches.Where(b => b.DepartmentId == departmentId).ToList();
        }

        public Department GetDepartmentById(int Id)
        {
            return _Context.Departments.FirstOrDefault(d => d.Id == Id);
        }

        public  DepartmentBranch GetDepartmentBranchById(int Id)
        {
            Department? Department = _Context.Departments.Include(d => d.Branches).FirstOrDefault(d => d.Id == Id);
            Branch? branch = _Context.Branches.FirstOrDefault(b => b.DepartmentId == Id);
            DepartmentBranch deptBranch = new DepartmentBranch();

            // Fix CS8602: Check for null before dereferencing
            if (Department != null)
            {
                deptBranch.DepartmentId = Department.Id; // Fix CS0019: Remove '?? 0', since Id is not nullable
                deptBranch.Name = Department.Name;
                deptBranch.ManagerName = Department.ManagerName;
                deptBranch.Location = Department.Location;
            }
            else
            {
                deptBranch.DepartmentId = 0;
                deptBranch.Name = string.Empty;
                deptBranch.ManagerName = null;
                deptBranch.Location = null;
            }

            // Assuming one branch per department for simplicity
            deptBranch.BranchName = branch != null ? branch.BranchName : default;

            return deptBranch;
        }

        public  Department GetDepartmentByName(string Name)
        {
            Department? Department = _Context.Departments.Include(d => d.Branches).FirstOrDefault(d => d.Name == Name);

            return Department;
        }

        public  bool AddDepartment(DepartmentBranch department)
        {
            Department dept = new Department()
            {
                Name = department.Name,
                ManagerName = department.ManagerName,
                Location = department.Location
            };

            _Context.Departments.Add(dept);

            if(_Context.SaveChanges() > 0)
            {
                //will only add the branch if the department is added successfully
                Branch branch = new Branch()
                {
                    BranchName = department.BranchName,
                    DepartmentId = dept.Id
                };
                _Context.Branches.Add(branch);

                if(_Context.SaveChanges() > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public  bool UpdateDepartment(DepartmentBranch department)
        {
            Department dept = new Department()
            {
                Id = department.DepartmentId,
                Name = department.Name,
                ManagerName = department.ManagerName,
                Location = department.Location
            };
            _Context.Departments.Update(dept);
            if (_Context.SaveChanges() > 0)
            {
                //will only update the branch if the department is updated successfully
                Branch branch = _Context.Branches.FirstOrDefault(b => b.DepartmentId == dept.Id);
                if(branch != null)
                {
                    branch.BranchName = department.BranchName;
                    _Context.Branches.Update(branch);
                    if (_Context.SaveChanges() > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool DeleteDepartment(Department department)
        {
            _Context.Departments.Update(department);
            return _Context.SaveChanges() > 0;
        }

        public bool DeleteAllDepartmentBranches(int DepartmentId)
        {
            List<Branch> branches = GetAllDepartmentBranches(DepartmentId);

            foreach (var branch in branches)
            {
                _Context.Branches.Remove(branch);
            }

            return _Context.SaveChanges() > 0;
        }
    }
}
