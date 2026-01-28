using CRM.Data;
using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Repositories
{
    public class AcademicRepository
    {
        private readonly CollegeContext _context;
        public AcademicRepository(CollegeContext context)
        {
            _context = context;
        }
        public List<Academy> GetAll()
        {
            return _context.Academy.ToList();
        }

        public void Add(Academy model)
        {
            _context.Academy.Add(model);
            _context.SaveChanges();
        }

        public void Update(Academy model)
        {
            _context.Academy.Update(model);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var item = _context.Academy.Find(id);
            if (item != null)
            {
                _context.Academy.Remove(item);
                _context.SaveChanges();
            }
        }

        public Academy GetById(int id)
        {
            return _context.Academy.Find(id);
        }


    }
}
