using CRM.Data;
using CRM.Models;
using CRM.ModelsForView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Repositories
{
    public class IssueAdmissionFormRepository
    {
        private readonly CollegeContext _context;
        public IssueAdmissionFormRepository(CollegeContext context)
        {
            _context = context;
        }

        public List<IssueAdmissionForm> GetAll()
        {
            return _context.IssueAdmissionForm.ToList();
        }

        public List<IssueAdmissionFormViewModel> ListForIssueAdmissionForm(
                          string session,
                          string @class,
                          string course,
                          string year,
                          string studentName, string status)
        {

            List<IssueAdmissionFormViewModel> resultList = new List<IssueAdmissionFormViewModel>();

            var result = from stu in _context.Student
                         join iaf in _context.IssueAdmissionForm
                 on stu.Id equals iaf.StudentId into admissionForms
                         from iaf in admissionForms.DefaultIfEmpty()
                         select new
                         {
                             StudentId = stu.Id,
                             StudentName = stu.StudentName,
                             FatherName = stu.FatherName,
                             MobileNo = stu.MobileNoOne,
                             Session = stu.Session,
                             Class = stu.Class,
                             Course = stu.Course,
                             Year = stu.Year,

                             AdmissionFormId = iaf != null ? iaf.Id : 0,
                             FormNo = iaf != null ? iaf.FormNo : 0,
                             FormStatus = iaf != null ? iaf.Status : null,
                             FormYear = iaf != null ? iaf.Year : null,
                             FormSession = iaf != null ? iaf.Session : null,
                             CreatedDate = iaf.CreatedDate,
                         };

            if (!string.IsNullOrWhiteSpace(studentName))
            {
                result = result.Where(x => x.StudentName.ToLower().Contains(studentName.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(session) && (session != "Select"))
            {
                result = result.Where(x => x.Session.ToLower().Contains(session.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(@class) && (@class != "Select"))
            {
                result = result.Where(x => x.Class.ToLower().Contains(@class.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(course) && (course != "Select"))
            {
                result = result.Where(x => x.Course.ToLower().Contains(course.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(year) && (year != "Select"))
            {
                result = result.Where(x => x.Year.ToLower().Contains(year.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(status) && (status != "Select"))
            {
                if(status == "NOT ISSUE")
                 result = result.Where(x => x.FormStatus == null);
                else
                  result = result.Where(x => x.FormStatus == status);
            }
            //---------------Result build-------------
            if (result != null)
            {
                foreach (var row in result)
                {
                    resultList.Add(new IssueAdmissionFormViewModel
                    {
                        StudentId = row.StudentId,
                        AdmissionFormNo = row.FormNo,
                        Year = row.Year,
                        Session = row.Session,
                        Class = row.Class,
                        Course = row.Course,
                        StudentName = row.StudentName,
                        FatherName = row.FatherName,
                        MobileNo = row.MobileNo,

                        // Issu Admission Form
                        AdmissionFormId = row.AdmissionFormId,
                        FormNo = row.FormNo,
                        FormStatus = row.FormStatus,
                        FormYear = row.FormYear,
                        FormSession = row.FormSession,
                        CreatedDate = row.CreatedDate.ToString("dd/MMM/yyyy"),
                         
                    });
                }
            }


            return resultList.ToList();
        }

        public int Add(IssueAdmissionForm model)
        {
            _context.IssueAdmissionForm.Add(model);
           var chekc =   _context.SaveChanges();
            return chekc;
        }

    }
}
