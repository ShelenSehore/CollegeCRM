using CRM.Data;
using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Repositories
{
    public class MstCourseRepository
    {
        private readonly CollegeContext _context;
        public MstCourseRepository(CollegeContext context)
        {
            _context = context;
        }

        public List<MstCourse> GetAll()
        {
            return _context.MstCourse.ToList();
        }
        public void Add(MstCourse model)
        {
            _context.MstCourse.Add(model);
            _context.SaveChanges();
        }

        public void Update(MstCourse model)
        {
            _context.MstCourse.Update(model);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var item = _context.MstCourse.Find(id);
            if (item != null)
            {
                _context.MstCourse.Remove(item);
                _context.SaveChanges();
            }
        }

        public MstCourse GetById(int id)
        {
            return _context.MstCourse.Find(id);
        }

    }
}
