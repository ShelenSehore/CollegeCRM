using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Interface
{
    public interface IEmployeeRepository
    {
        public List<Employee> GetAll();
        public int Add(Employee model);
    }
}
