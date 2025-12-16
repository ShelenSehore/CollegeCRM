using CRM.Data;
using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Repositories
{
    public class StudentRegistrationRepository
    {
        private readonly CollegeContext _context;

        public StudentRegistrationRepository(CollegeContext context)
        {
            _context = context;
        }

        public List<StudentRegistration> GetAll()
        {
            return _context.StudentRegistration.ToList();
        }
        public void Add(StudentRegistration model)
        {
            _context.StudentRegistration.Add(model);
            _context.SaveChanges();
        }

        public void Update(StudentRegistration model)
        {
            _context.StudentRegistration.Update(model);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var item = _context.StudentRegistration.Find(id);
            if (item != null)
            {
                _context.StudentRegistration.Remove(item);
                _context.SaveChanges();
            }
        }

        public StudentRegistration GetById(int id)
        {
            return _context.StudentRegistration.Find(id);
        }

    }
}
