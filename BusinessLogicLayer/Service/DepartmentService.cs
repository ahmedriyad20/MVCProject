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
        private readonly UniversityContext _context;
        public DepartmentService(UniversityContext context)
        {
            _context = context;
        }
        public  List<Department> GetAllDepartments()
        {
            List<Department> Departments = _context.Departments.Include(d => d.Branches).ToList();

            return Departments;
        }

        public List<Branch> GetAllDepartmentBranches(int departmentId)
        {
            return _context.Branches.Where(b => b.DepartmentId == departmentId).ToList();
        }
        public  DepartmentBranch GetDepartmentById(int Id)
        {
            Department? Department = _context.Departments.Include(d => d.Branches).FirstOrDefault(d => d.Id == Id);
            //List<Branch>? branch = _context.Branches.Where(b => b.DepartmentId == Id).ToList(); //will return all branches for the department
            Branch? branch = _context.Branches.FirstOrDefault(b => b.DepartmentId == Id);
            DepartmentBranch deptBranch = new DepartmentBranch()
            {
                DepartmentId = Department.Id,
                Name = Department.Name,
                ManagerName = Department.ManagerName,
                Location = Department.Location,
                BranchName = branch.BranchName // Assuming one branch per department for simplicity
            };
            return deptBranch;
        }

        public  Department GetDepartmentByName(string Name)
        {
            Department? Department = _context.Departments.Include(d => d.Branches).FirstOrDefault(d => d.Name == Name);

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

            _context.Departments.Add(dept);

            if(_context.SaveChanges() > 0)
            {
                //will only add the branch if the department is added successfully
                Branch branch = new Branch()
                {
                    BranchName = department.BranchName,
                    DepartmentId = dept.Id
                };
                _context.Branches.Add(branch);

                if(_context.SaveChanges() > 0)
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
            _context.Departments.Update(dept);
            if (_context.SaveChanges() > 0)
            {
                //will only update the branch if the department is updated successfully
                Branch branch = _context.Branches.FirstOrDefault(b => b.DepartmentId == dept.Id);
                if(branch != null)
                {
                    branch.BranchName = department.BranchName;
                    _context.Branches.Update(branch);
                    if (_context.SaveChanges() > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
