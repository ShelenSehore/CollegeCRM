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

        public bool UpdateHistoryDetail(StudentHistory model)
        {
            var student = _context.StudentHistory.FirstOrDefault(x => x.StudentId == model.StudentId && x.Session == model.Session && x.Classs== model.Classs && x.Course== model.Course && x.Year==model.Year);

            if (student != null)
            {
                try { 
                
                student.AdmissionForm = model.AdmissionForm;
               // student.AdmissionDate = model.AdmissionDate;
                student.ScholerNo = model.ScholerNo;
                student.RegPvt = model.RegPvt;
                student.NewOld = model.NewOld;
                student.EnrolNo = model.Session;
                student.Session = model.Session;
                student.StudentName = model.StudentName;
                student.FatherName = model.FatherName;
                student.FatherMobileNo = model.FatherMobileNo;
                student.MotherName = model.MotherName;
                student.MobileNo = model.MobileNo;
              //  student.DOB = model.DOB;
                student.Gender = model.Gender;
                student.Minority = model.Minority;
                student.Cast = model.Cast;
                student.AdharNo = model.AdharNo;
                student.Medium = model.Medium;
                student.EnrolNo = model.EnrolNo;
                student.RollNo = model.RollNo;
                student.Minority = model.Minority;
                student.AbcId = model.AbcId;
                student.SamagraId = model.SamagraId;
                student.Address = model.Address;
                student.TCIssue = model.TCIssue;
                student.PH = model.PH;
                student.ExamFormSubmited = model.ExamFormSubmited;
                student.SubCode = model.SubCode;
                student.PassoutTC = model.PassoutTC;

                student.UpdateDate = DateTime.Now;
                student.UpdateBy = "Admin";
                var status = _context.SaveChanges();
                return true;

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return false;
        }



        public bool UpdateHistoryDetailForHistoryPage(StudentHistory model)
        {
            var student = _context.StudentHistory.FirstOrDefault(x => x.StudentHistoryId == model.StudentHistoryId);

            if (student != null)
            {
                student.StudentName = model.StudentName;
                student.FatherName = model.FatherName;
                student.MotherName = model.MotherName;
                student.MobileNo = model.MobileNo;
                student.FatherMobileNo = model.FatherMobileNo;
                student.DOB = model.DOB;
                student.Gender = model.Gender;
                student.Minority = model.Minority;
                student.Cast = model.Cast;
                student.AdharNo = model.AdharNo;
                student.Medium = model.Medium;
                student.EnrolNo = model.EnrolNo;
                student.RollNo = model.RollNo;
                student.Minority = model.Minority;
                student.AbcId = model.AbcId;
                student.SamagraId = model.SamagraId;
                student.Address = model.Address;
                student.TCIssue = model.TCIssue;
                student.PH = model.PH;
                student.UpdateDate = DateTime.Now;
                student.UpdateBy = "Admin";
                var status = _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool UpdateHisotryCollegeDetailForHistoryPage(StudentHistory model)
        {
            var student = _context.StudentHistory.FirstOrDefault(x => x.StudentHistoryId == model.StudentHistoryId);

            if (student != null)
            {
                student.AdmissionForm = model.AdmissionForm;
                student.AdmissionDate = model.AdmissionDate;
                student.NewOld = model.NewOld;
                student.Medium = model.Medium;
                //student.Session = model.Session;
                //student.Class = model.Class;
                //student.Course = model.Course;
                //student.Year = model.Year;
                student.EnrolNo = model.EnrolNo;
                student.RollNo = model.RollNo;
                student.ScholerNo = model.ScholerNo;
                student.SubCode = model.SubCode;
                student.RegPvt = model.RegPvt;
                student.ExamFormSubmited = model.ExamFormSubmited;
                student.Result = model.Result;
                student.UpdateDate = model.UpdateDate;
                student.UpdateBy = model.UpdateBy;

                var status = _context.SaveChanges();
                return true;
            }
            return false;
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

        public List<StudentHistory> GetListByStuId(int id)
        {
            return _context.StudentHistory.Where(x => x.StudentId == id).ToList();
        }

        public StudentHistory GetById(int id)
        {
            return _context.StudentHistory.Where(x => x.StudentHistoryId == id).FirstOrDefault();
        }

        public bool SaveResultList(string ids, string result)
        {
            if (string.IsNullOrEmpty(ids)) return false;

            var idList = ids.Split(',')
                   .Select(int.Parse)
                   .ToList();

            var studentsHistory = _context.StudentHistory.Where(x => idList.Contains(x.StudentHistoryId)).ToList();

            if (studentsHistory.Any())
            {
                foreach (var history in studentsHistory)
                {
                    history.Result = result;
                }
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        //------Download History Data---------------
        public List<StudentHistory> GetByStudentHistoryPage(
                          string session,
                          string @class,
                          string course,
                          string year,
                          string studentName, string rollNum, string result, string formNo, 
                          string enrollmentNo, string regulerprivate, string newoldex)
        {
            IQueryable<StudentHistory> query = _context.StudentHistory;

            if (!string.IsNullOrWhiteSpace(studentName))
            {
                query = query.Where(x => x.StudentName.ToLower().Contains(studentName.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(formNo))
            {
                query = query.Where(x => x.AdmissionForm== Convert.ToInt32(formNo));
            }
            if (!string.IsNullOrWhiteSpace(enrollmentNo))
            {
                query = query.Where(x => x.EnrolNo.ToLower().Contains(enrollmentNo.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(rollNum))
            {
                query = query.Where(x => x.RollNo.ToLower().Contains(rollNum.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(session) && (session != "Select"))
            {
                query = query.Where(x => x.Session.ToLower().Contains(session.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(@class) && (@class != "Select"))
            {
                query = query.Where(x => x.Classs.ToLower().Contains(@class.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(course) && (course != "Select"))
            {
                query = query.Where(x => x.Course.ToLower().Contains(course.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(year) && (year != "Select"))
            {
                query = query.Where(x => x.Year.ToLower().Contains(year.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(result) && (result != "Select"))
            {
                query = query.Where(x => x.Result.ToLower().Contains(result.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(regulerprivate) && (regulerprivate != "Select"))
            {
                query = query.Where(x => x.RegPvt.ToLower().Contains(regulerprivate.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(newoldex) && (newoldex != "Select"))
            {
                query = query.Where(x => x.NewOld.ToLower().Contains(newoldex.ToLower()));
            }

            return query.ToList();
        }

    }
}
