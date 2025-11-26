using CRM.Data;
using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Repositories
{
    public class MstSubjectRepository
    {
        private readonly CollegeContext _context;

        public MstSubjectRepository(CollegeContext context)
        {
            _context = context;
        }

        public List<MstSubject> GetAll()
        {
            return _context.MstSubject.ToList();
        }
        public void Add(MstSubject model)
        {
            _context.MstSubject.Add(model);
            _context.SaveChanges();
        }

        public void Update(MstSubject model)
        {
            _context.MstSubject.Update(model);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var item = _context.MstSubject.Find(id);
            if (item != null)
            {
                _context.MstSubject.Remove(item);
                _context.SaveChanges();
            }
        }

        public MstSubject GetById(int id)
        {
            return _context.MstSubject.Find(id);
        }


    }
}
