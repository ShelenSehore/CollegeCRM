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
                student.StudentName = model.StudentName;
                student.FatherName = model.FatherName;
                student.MotherName = model.MotherName;
                student.MobileNo = model.MobileNo;
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
