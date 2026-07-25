using CRM.Data;
using CRM.Interface;
using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly CollegeContext _context;
        public EmployeeRepository(CollegeContext context)
        {
            _context = context;
        }

        public List<Employee> GetAll()
        {
            return _context.Employee.ToList();
        }

        public int Add(Employee model)
        {
            _context.Employee.Add(model);
            var chekc = _context.SaveChanges();
            return chekc;
        }


    }
}
