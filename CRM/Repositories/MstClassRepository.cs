using CRM.Data;
using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Repositories
{
    public class MstClassRepository
    {
        private readonly CollegeContext _context;
            public MstClassRepository(CollegeContext context)
            {
                _context = context;
            }

        public List<MstClass> GetAll()
        {
            return _context.MstClass.ToList();
        }

        public void Add(MstClass model)
        {
            _context.MstClass.Add(model);
            _context.SaveChanges();
        }

        public void Update(MstClass model)
        {
            _context.MstClass.Update(model);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var item = _context.MstClass.Find(id);
            if (item != null)
            {
                _context.MstClass.Remove(item);
                _context.SaveChanges();
            }
        }

        public MstClass GetById(int id)
        {
            return _context.MstClass.Find(id);
        }

    }
}
