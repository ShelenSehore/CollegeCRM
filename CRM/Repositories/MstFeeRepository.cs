using CRM.Data;
using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Repositories
{
    public class MstFeeRepository
    {
        private readonly CollegeContext _context;
        public MstFeeRepository(CollegeContext context)
        {
            _context = context;
        }
        public List<MstFee> GetAll()
        {
            return _context.MstFee.ToList();
        }

        public void Add(MstFee model)
        {
            _context.MstFee.Add(model);
            _context.SaveChanges();
        }

        public void Update(MstFee model)
        {
            _context.MstFee.Update(model);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var item = _context.MstFee.Find(id);
            if (item != null)
            {
                _context.MstFee.Remove(item);
                _context.SaveChanges();
            }
        }

        public MstFee GetById(int id)
        {
            return _context.MstFee.Find(id);
        }

        public MstFee GetFeeByClasssCouseSessionYearNewOld(string Classs, string Course, string Session, string Year, string NewOld)
        {
            return _context.MstFee.FirstOrDefault(x => x.Year == Year  && x.NewOld == NewOld && x.Course == Course && x.Subject == Classs);


        }

    }
}
