using CRM.Data;
using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Repositories
{
    public class StudentRepository
    {
        private readonly CollegeContext _context;

        public StudentRepository(CollegeContext context)
        {
            _context = context;
        }

        public List<Student> GetAll()
        {
            return _context.Student.ToList();
        }
        public void Add(Student model)
        {
            _context.Student.Add(model);
            _context.SaveChanges();
        }

        public void Update(Student model)
        {
            _context.Student.Update(model);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var item = _context.Student.Find(id);
            if (item != null)
            {
                _context.Student.Remove(item);
                _context.SaveChanges();
            }
        }

        public Student GetById(int id)
        {
            return _context.Student.Find(id);
        }


    }
}
