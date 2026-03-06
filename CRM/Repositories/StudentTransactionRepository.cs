using CRM.Data;
using CRM.Models;
using CRM.ModelsForView;
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

        public List<ReportTransactionViewModel> FilterList(string had, string paymentMode, string reciptNo, 
                                                          string fromDate, string toDate, string name, string session,
                                                          string year, string classes, string course)
        {
            try
            {
                List<ReportTransactionViewModel> resultList = new List<ReportTransactionViewModel>();
               
                var query = from tr in _context.StudentTransaction
                            join st in _context.Student
                            on tr.StudentId equals st.Id
                            select new
                            {
                                tr.Id,
                                tr.StudentId,
                                tr.StudentFeeId,
                                tr.Head,
                                tr.RecBookNo,
                                tr.RecNumber,
                                tr.Amount,
                                tr.PaymentMode,
                                tr.TransactionNo,
                                tr.CreatedBy,
                                tr.CreateDateTime,
                                st.AdmissionFormNo,
                                st.Year,
                                st.Class,
                                st.Course,
                                st.Session,
                                st.StudentName,
                                st.FatherName,
                                st.MobileNoOne,
                                st.NewOld,
                                st.RegEx
                            };

                if (!string.IsNullOrEmpty(had) && (had != "Select"))
                    query = query.Where(x => x.Head == had);

                if (!string.IsNullOrEmpty(paymentMode) && (paymentMode != "Select"))
                    query = query.Where(x => x.PaymentMode == paymentMode);

                if (!string.IsNullOrEmpty(reciptNo))
                    query = query.Where(x => x.RecNumber == reciptNo);

                if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                {
                    query = query.Where(x => x.CreateDateTime > Convert.ToDateTime(fromDate) && x.CreateDateTime < Convert.ToDateTime(fromDate));
                }
                    

                if (!string.IsNullOrEmpty(name))
                    query = query.Where(x => x.StudentName.Contains(name));

                if (!string.IsNullOrEmpty(session) && (session != "Select"))
                    query = query.Where(x => x.Session == session);

                if (!string.IsNullOrEmpty(year) && (year != "Select"))
                    query = query.Where(x => x.Year == year);

                if (!string.IsNullOrEmpty(classes) && (classes != "Select"))
                    query = query.Where(x => x.Class == classes);

                if (!string.IsNullOrEmpty(course) && (course != "Select"))
                    query = query.Where(x => x.Course == course);

                //---------------Result build-------------
                if (query != null)
                {
                    foreach (var row in query)
                    {
                        ReportTransactionViewModel rowObj = new ReportTransactionViewModel();
                        rowObj.Id = row.Id;
                        rowObj.StudentId = row.StudentId;
                        rowObj.StudentFeeId = row.StudentFeeId;
                        rowObj.Head = row.Head;
                        rowObj.RecBookNo = row.RecBookNo;
                        rowObj.RecNumber = row.RecNumber;
                        rowObj.Amount = row.Amount;
                        rowObj.PaymentMode = row.PaymentMode;
                        rowObj.TransactionNo = row.TransactionNo;
                        rowObj.AdmissionFormNo = row.AdmissionFormNo;
                        rowObj.Year = row.Year;
                        rowObj.Session = row.Session;
                        rowObj.Class = row.Class;
                        rowObj.Course = row.Course;
                        rowObj.RegEx = row.RegEx;
                        rowObj.NewOld = row.NewOld;
                        rowObj.StudentName = row.StudentName;
                        rowObj.FatherName = row.FatherName;
                        rowObj.MobileNoOne = row.MobileNoOne;

                        resultList.Add(rowObj);
                    }
                    return resultList;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }


        public List<ReportTransactionViewModel> DFCList(string had, string paymentMode, string reciptNo,
                                                        string fromDate, string toDate)
        {
            try
            {
                List<ReportTransactionViewModel> resultList = new List<ReportTransactionViewModel>();



                // 1. Start with the base query
                var query = _context.StudentTransaction.AsQueryable();

                // 2. Add dynamic "Where" conditions based on parameters
                if (!string.IsNullOrEmpty(had) && (had != "Select"))
                    query = query.Where(x => x.Head == had);

                if (!string.IsNullOrEmpty(paymentMode) && (paymentMode != "Select"))
                    query = query.Where(x => x.PaymentMode == paymentMode);

              
                if (DateTime.TryParse(fromDate, out DateTime start))
                    query = query.Where(x => x.CreateDateTime >= start);

                if (DateTime.TryParse(toDate, out DateTime end))
                    query = query.Where(x => x.CreateDateTime <= end);

                // 3. Final GroupBy and Projection
                var result = query
                    .GroupBy(x => x.Head)
                    .Select(g => new ReportTransactionViewModel // Projecting to your ViewModel
                    {
                        Head = g.Key,
                        TotalAmount = g.Sum(x => x.Amount)
                    })
                    .ToList();



                //---------------Result build-------------
                if (result != null)
                {
                    foreach (var row in result)
                    {
                        ReportTransactionViewModel rowObj = new ReportTransactionViewModel();
                        rowObj.Id = row.Id;
                       
                        rowObj.Head = row.Head;
                        
                        rowObj.TotalAmount = row.TotalAmount;
                       

                        resultList.Add(rowObj);
                    }
                    return resultList;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }


        public List<ReportTransactionViewModel> FilterStudentList(string name, string session,
                                                        string year, string classes, string course)
        {
            try
            {
                List<ReportTransactionViewModel> resultList = new List<ReportTransactionViewModel>();

                var query = from fee in _context.StudentFee
                            join st in _context.Student
                            on fee.StudentId equals st.Id
                            select new
                            {
                                fee.Id,
                                fee.StudentId,
                               
                                fee.Year,
                                fee.Course,
                                fee.Class,
                                fee.Session,
                                fee.NewOld,
                                fee.NewStudentFee,
                                fee.CMoney,
                                fee.TutionFee,
                                fee.OtherFee,
                                fee.TotalFee,
                                fee.TotalFeeAfterDiscount,
                                fee.CMoneyPaidOrNot,
                                fee.PaidAmount,
                                fee.Scholership,
                                fee.DisBy,
                                fee.DisResion,
                                st.StudentName,
                                st.FatherName,
                                st.MobileNoOne
                               
                            };


                if (!string.IsNullOrEmpty(name))
                    query = query.Where(x => x.StudentName.Contains(name));

                if (!string.IsNullOrEmpty(session) && (session != "Select"))
                    query = query.Where(x => x.Session == session);

                if (!string.IsNullOrEmpty(year) && (year != "Select"))
                    query = query.Where(x => x.Year == year);

                if (!string.IsNullOrEmpty(classes) && (classes != "Select"))
                    query = query.Where(x => x.Class == classes);

                if (!string.IsNullOrEmpty(course) && (course != "Select"))
                    query = query.Where(x => x.Course == course);

                //---------------Result build-------------
                if (query != null)
                {
                    foreach (var row in query)
                    {
                        ReportTransactionViewModel rowObj = new ReportTransactionViewModel();
                        rowObj.Id = row.Id;
                        rowObj.StudentId = row.StudentId;
                        rowObj.StudentFeeId = row.StudentFeeId;
                        rowObj.Head = row.Head;
                        rowObj.RecBookNo = row.RecBookNo;
                        rowObj.RecNumber = row.RecNumber;
                        rowObj.Amount = row.Amount;
                        rowObj.PaymentMode = row.PaymentMode;
                        rowObj.TransactionNo = row.TransactionNo;
                        rowObj.AdmissionFormNo = row.AdmissionFormNo;
                        rowObj.Year = row.Year;
                        rowObj.Session = row.Session;
                        rowObj.Class = row.Class;
                        rowObj.Course = row.Course;
                        rowObj.RegEx = row.RegEx;
                        rowObj.NewOld = row.NewOld;
                        rowObj.StudentName = row.StudentName;
                        rowObj.FatherName = row.FatherName;
                        rowObj.MobileNoOne = row.MobileNoOne;

                        resultList.Add(rowObj);
                    }
                    return resultList;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

    }
}
