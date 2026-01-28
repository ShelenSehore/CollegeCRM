using CRM.Data;
using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Repositories
{
    public class MstYearRepository
    {
        private readonly CollegeContext _context;

        public MstYearRepository(CollegeContext context)
        {
            _context = context;
        }

        public List<MstYear> GetAll()
        {
            return _context.MstYear.ToList();
        }

        public void Add(MstYear model)
        {
            _context.MstYear.Add(model);
            _context.SaveChanges();
        }

        public void Update(MstYear model)
        {
            _context.MstYear.Update(model);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var item = _context.MstYear.Find(id);
            if (item != null)
            {
                _context.MstYear.Remove(item);
                _context.SaveChanges();
            }
        }

        public MstYear GetById(int id)
        {
            return _context.MstYear.Find(id);
        }

    }
}
