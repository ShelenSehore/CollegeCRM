using CRM.Data;
using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Repositories
{
    public class MstSessionRepository
    {
        private readonly CollegeContext _context;

        public MstSessionRepository(CollegeContext context)
        {
            _context = context;
        }
        public List<MstSession> GetAll()
        {
            return _context.MstSession.ToList();
        }

        public void Add(MstSession model)
        {
            _context.MstSession.Add(model);
            _context.SaveChanges();
        }

        public void Update(MstSession model)
        {
            _context.MstSession.Update(model);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var item = _context.MstSession.Find(id);
            if (item != null)
            {
                _context.MstSession.Remove(item);
                _context.SaveChanges();
            }
        }

        public MstSession GetById(int id)
        {
            return _context.MstSession.Find(id);
        }




    }
}
