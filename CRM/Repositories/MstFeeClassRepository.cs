using CRM.Data;
using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Repositories
{
    public class MstFeeClassRepository
    {
        private readonly CollegeContext _context;

        public MstFeeClassRepository(CollegeContext context)
        {
            _context = context;
        }

        public List<MstFeeClass> GetAll()
        {
            return _context.MstFeeClass.ToList();
        }

        public void Add(MstFeeClass model)
        {
            _context.MstFeeClass.Add(model);
            _context.SaveChanges();
        }

        public void Update(MstFeeClass model)
        {
            _context.MstFeeClass.Update(model);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var item = _context.MstFeeClass.Find(id);
            if (item != null)
            {
                _context.MstFeeClass.Remove(item);
                _context.SaveChanges();
            }
        }

        public MstFeeClass GetById(int id)
        {
            return _context.MstFeeClass.Find(id);
        }

    }
}
