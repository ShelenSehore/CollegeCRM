using CRM.Data;
using CRM.Models;
using CRM.ModelsForView;
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

        public bool UpdateOnlyFeeAmount(StudentFee model)
        {
            var studentFee = _context.StudentFee.FirstOrDefault(x => x.Id == model.Id);
            if (studentFee != null) 
            {
                studentFee.PaidAmount = studentFee.PaidAmount + model.PaidAmount;

                if (!string.IsNullOrEmpty(model.CMoneyPaidOrNot))
                    studentFee.CMoneyPaidOrNot = model.CMoneyPaidOrNot;
                else
                    studentFee.CMoneyPaidOrNot = "NO";

                var status = _context.SaveChanges();
                return true;
            }
            return false;
        }

        public StudentFee GetFeeIdByClassSessionYear(int studentId ,string payClass, string payCourse, string payYear, string paySession)
        {
            var studentFee = _context.StudentFee.FirstOrDefault(x => x.StudentId == studentId && x.Class== payClass && x.Course== payCourse  && x.Year== payYear && x.Session == paySession);
            
            return studentFee;
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

       
        public StudentFee GetFeeByClasssCouseSessionYearNewOld(int studentId, string classes, string course,  string session, string year, string newOld)
        {
            return _context.StudentFee.FirstOrDefault(x => x.StudentId == studentId && x.Class == classes && x.Course == course && x.Year == year && x.Session == session);
        }

        public List<StudentFee> GetFeeByStudentID(int studentId)
        {
            return _context.StudentFee.Where(x => x.StudentId == studentId).ToList();
        }


        public bool FeeConsessionAfterAdmission(StudentFee model)
        {
            var studentFee = _context.StudentFee.FirstOrDefault(x => x.Id == model.Id);
            if (studentFee != null)
            {
                studentFee.DisBy = model.DisBy;
                studentFee.DisResion = model.DisResion;
                studentFee.Scholership = studentFee.Scholership +  model.Scholership;
                studentFee.TotalFeeAfterDiscount = model.TotalFeeAfterDiscount;
                var status = _context.SaveChanges();
                return true;
            }
            return false;
        }


        //----------------Recipt List-----------------

        public List<ReceiptListViewModel> GetReceiptList(string session, string classes, string course, string year, string name)
        {
            List<ReceiptListViewModel> returnList = new List<ReceiptListViewModel>();
            try { 

            

           
            returnList = _context.StudentTransaction
                                   .Join(
                                       _context.Student,
                                       st => st.StudentId,
                                       s => s.Id,
                                       (st, s) => new { st, s }
                                   )
                                   .GroupBy(x => new
                                   {
                                       x.st.StudentId,
                                       x.s.StudentName,
                                       x.s.FatherName,
                                       x.s.Class,
                                       x.s.Course,
                                       x.s.Year,
                                       x.st.StudentFeeId,
                                       x.st.RecBookNo,
                                       x.st.RecNumber,
                                       x.st.PaymentMode,
                                       x.st.CreateDateTime
                                   })
                                   .Select(g => new ReceiptListViewModel
                                   {
                                       StudentId = g.Key.StudentId,
                                       StudentName = g.Key.StudentName,
                                       FatherName = g.Key.FatherName,
                                       Class = g.Key.Class,
                                       Course = g.Key.Course,
                                       Year = g.Key.Year,
                                       StudentFeeId = g.Key.StudentFeeId,
                                       RecBookNo = g.Key.RecBookNo,
                                       RecNumber = g.Key.RecNumber,
                                       PaymentMode = g.Key.PaymentMode,
                                       Count = g.Count(),
                                       TotalAmount = g.Sum(x => x.st.Amount),
                                       CreateDateTime = g.Key.CreateDateTime
                                   })
                                   .OrderBy(x => x.RecBookNo)
                                   .ThenBy(x => x.RecNumber)
                                   .ToList();
            }
            catch (Exception ex)
            { 
            }
            return returnList;
        }




    }
}
