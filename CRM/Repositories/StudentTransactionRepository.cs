using CRM.Data;
using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Repositories
{
    public class StudentTransactionRepository
    {
        private readonly CollegeContext _context;
        public StudentTransactionRepository(CollegeContext context)
        {
            _context = context;
        }

        public List<StudentTransaction> GetAll()
        {
            return _context.StudentTransaction.ToList();
        }

        public void Add(Academy model)
        {
            _context.Academy.Add(model);
            _context.SaveChanges();
        }

        public int SaveAndGetId(StudentTransaction model)
        {
            try
            {
                _context.StudentTransaction.Add(model);
                _context.SaveChanges();
                return model.Id;
            }
            catch (Exception ex)
            {
            }
            return 0;
        }



        public void Update(StudentTransaction model)
        {
            _context.StudentTransaction.Update(model);
            _context.SaveChanges();
        }

        public void BulkSave(List<StudentTransaction> model)
        {
            _context.StudentTransaction.AddRange(model);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var item = _context.StudentTransaction.Find(id);
            if (item != null)
            {
                _context.StudentTransaction.Remove(item);
                _context.SaveChanges();
            }
        }

        public StudentTransaction GetById(int id)
        {
            return _context.StudentTransaction.Find(id);
        }

        public List<StudentTransaction> GetAllByStudentIDandFeeID(int studentID, int FeeId)
        {
            return _context.StudentTransaction.Where(x=>x.StudentId == studentID).OrderByDescending(x => x.Id).ToList();
        }


    }
}
