using CRM.Data;
using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Repositories
{
    public class StudentRegitrationFeeRepository
    {
        private readonly CollegeContext _context;
        public StudentRegitrationFeeRepository(CollegeContext context)
        {
            _context = context;
        }

        public List<StudentRegitrationFee> GetAll()
        {
            return _context.StudentRegitrationFee.ToList();
        }

        public void Add(StudentRegitrationFee model)
        {
            try
            {
                _context.StudentRegitrationFee.Add(model);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int SaveAndGetId(StudentRegitrationFee model)
        {
            try
            {
                _context.StudentRegitrationFee.Add(model);
                _context.SaveChanges();
                return model.Id;
            }
            catch (Exception ex)
            {
            }
            return 0;
        }


        public StudentRegitrationFee GetById(int id)
        {
            return _context.StudentRegitrationFee.Where(x => x.StudentId == id).FirstOrDefault();
        }

      


    }
}
