using CRM.Data;
using CRM.Models;
using CRM.ModelsForView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Repositories
{
    public class StudentRegistrationRepository
    {
        private readonly CollegeContext _context;

        public StudentRegistrationRepository(CollegeContext context)
        {
            _context = context;
        }

        public List<StudentRegistration> GetAll()
        {
            return _context.StudentRegistration.ToList();
        }

        public int GetLatestId()
        {
            var getRecord = _context.StudentRegistration.OrderByDescending(x => x.Id).FirstOrDefault();
            return getRecord.Id;
        }


        public void Add(StudentRegistration model)
        {
            _context.StudentRegistration.Add(model);
            _context.SaveChanges();
           
        }

        public int SaveAndGetId(StudentRegistration model)
        {
            _context.StudentRegistration.Add(model);
            var IsSave = _context.SaveChanges();
            return model.Id;
        }

        public void Update(StudentRegistration model)
        {
            _context.StudentRegistration.Update(model);
            _context.SaveChanges();
        }

        public bool IsMoved(bool status, int studentRegId)
        {
            var studentRegisStatus = _context.StudentRegistration.FirstOrDefault(x => x.Id == studentRegId);
            if (studentRegisStatus != null)
            {
                studentRegisStatus.IsMove = status;
                var statuss = _context.SaveChanges();
                return true;
            }
            return false;
        }

        public void Delete(int id)
        {
            var item = _context.StudentRegistration.Find(id);
            if (item != null)
            {
                _context.StudentRegistration.Remove(item);
                _context.SaveChanges();
            }
        }

        public StudentRegistration GetById(int id)
        {
            return _context.StudentRegistration.Find(id);
        }

        public List<StudentRegistration> FilterList(string name, string classes, string year, string course, string regPvt)
        {
            IQueryable<StudentRegistration> query = _context.StudentRegistration;

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(name.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(classes) && (classes != "Select"))
            {
                query = query.Where(x => x.Year.ToLower().Contains(classes.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(year) && (year != "Select"))
            {
                query = query.Where(x => x.Year.ToLower().Contains(year.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(course) && (course != "Select"))
            {
                query = query.Where(x => x.Course.ToLower().Contains(course.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(regPvt) && (regPvt != "Select"))
            {
                query = query.Where(x => x.RegPvt.ToLower().Contains(regPvt.ToLower()));
            }
            query = query.Where(x => x.IsMove == false);
            return query.ToList();
            //return _context.StudentRegistration.ToList(); 
        }


        //-------------downLoad Excel--------------
        public List<StudentRegDownload> DownLoadExcelFilterList(string name, string classes, string year, string course, string regPvt)
        {
           // IQueryable<StudentRegistration> query = _context.StudentRegistration;

            var query = from stu in _context.StudentRegistration
                        join regFee in _context.StudentRegitrationFee
                        on stu.Id equals regFee.StudentId
                        select new StudentRegDownload {
                           Id=  stu.Id,
                            RegNo=  stu.RegNo,
                            FormNo = stu.FormNo,
                            SchoNo = stu.SchoNo,
                            Session = stu.Session,
                            Subject = stu.Subject,
                            Course = stu.Course,
                            Sem = stu.Sem,
                            RegPvt = stu.RegPvt,
                            Status = stu.Status,
                            Name = stu.Name,
                            FatherName = stu.FatherName,
                            MotherName = stu.MotherName,
                            DOB = stu.DOB,
                            Caste = stu.Caste,
                            Gender = stu.Gender,
                            MobileNo = stu.MobileNo,
                            CreateDate = stu.CreateDate,
                            IsMove = stu.IsMove,
                            NewStudentFee = regFee.NewStudentFee,
                            CMoney = regFee.CMoney,
                            TutionFee = regFee.TutionFee,
                            OtherFee = regFee.OtherFee,
                            TotalFee = regFee.TotalFee,
                            TotalFeeCM = regFee.TotalFeeCM,
                            TotalFeeAfterDiscount = regFee.TotalFeeAfterDiscount,
                            CMoneyPaidOrNot = regFee.CMoneyPaidOrNot,
                            PaidAmount = regFee.PaidAmount,
                            DisByExcel = regFee.DisBy,
                            DisResionExcel = regFee.DisResion,
                            ScholershipExcel = regFee.Scholership
                        };


            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(name.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(classes) && (classes != "Select"))
            {
                query = query.Where(x => x.Year.ToLower().Contains(classes.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(year) && (year != "Select"))
            {
                query = query.Where(x => x.Year.ToLower().Contains(year.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(course) && (course != "Select"))
            {
                query = query.Where(x => x.Course.ToLower().Contains(course.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(regPvt) && (regPvt != "Select"))
            {
                query = query.Where(x => x.RegPvt.ToLower().Contains(regPvt.ToLower()));
            }
            query = query.Where(x => x.IsMove == false);
            return query.ToList();
            //return _context.StudentRegistration.ToList(); 
        }


    }
}
