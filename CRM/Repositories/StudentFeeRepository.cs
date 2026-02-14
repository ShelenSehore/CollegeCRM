using CRM.Data;
using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Repositories
{
    public class StudentFeeRepository
    {
        private readonly CollegeContext _context;
        public StudentFeeRepository(CollegeContext context)
        {
            _context = context;
        }

        public List<StudentFee> GetAll()
        {
            return _context.StudentFee.ToList();
        }

        public void Add(StudentFee model)
        {
            try {
                _context.StudentFee.Add(model);
                _context.SaveChanges();
            }
            catch (Exception ex) {
                throw ex;
            }
            
        }

        public int SaveAndGetId(StudentFee model)
        {
            try
            {
                _context.StudentFee.Add(model);
                _context.SaveChanges();
                return model.Id;
            }
            catch (Exception ex)
            {
            }
            return 0;
        }

        public void Update(StudentFee model)
        {
            _context.StudentFee.Update(model);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var item = _context.StudentFee.Find(id);
            if (item != null)
            {
                _context.StudentFee.Remove(item);
                _context.SaveChanges();
            }
        }

        public StudentFee GetById(int id)
        {
            return _context.StudentFee.Find(id);
        }

        //public StudentFee GetStudentFeeDetailB(int studentId, string classes, string course, string year, string session, string newOld )
        //{

        //    return _context.StudentFee.Where(x => x.StudentId == studentId && x.Class == classes).FirstOrDefault();
        //}
        public StudentFee GetFeeByClasssCouseSessionYearNewOld(int studentId, string classes, string course,  string session, string year, string newOld)
        {
            return _context.StudentFee.FirstOrDefault(x => x.StudentId == studentId && x.Class == classes && x.Course == course && x.Year == year && x.Session == session);
        }

    }
}
