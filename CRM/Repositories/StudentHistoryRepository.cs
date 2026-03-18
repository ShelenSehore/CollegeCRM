using CRM.Data;
using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Repositories
{
    public class StudentHistoryRepository
    {
        private readonly CollegeContext _context;
        public StudentHistoryRepository(CollegeContext context)
        {
            _context = context;
        }

        public List<StudentHistory> GetAll()
        {
            return _context.StudentHistory.ToList();
        }

        public void Update(StudentHistory model)
        {
            _context.StudentHistory.Update(model);
            _context.SaveChanges();
        }


        public void Add(StudentHistory model)
        {
            try
            {
                _context.StudentHistory.Add(model);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int SaveAndGetId(StudentHistory model)
        {
            try
            {
                _context.StudentHistory.Add(model);
                _context.SaveChanges();
                return model.StudentHistoryId;
            }
            catch (Exception ex)
            {
            }
            return 0;
        }


        public StudentHistory GetById(int id)
        {
            return _context.StudentHistory.Where(x => x.StudentHistoryId == id).FirstOrDefault();
        }
    }
}
