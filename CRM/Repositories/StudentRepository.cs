using CRM.Data;
using CRM.Models;
using CRM.ModelsForView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Repositories
{
    public class StudentRepository
    {
        private readonly CollegeContext _context;

        public StudentRepository(CollegeContext context)
        {
            _context = context;
        }

        public List<Student> GetAll()
        {
            return _context.Student.ToList();
        }
        public void Add(Student model)
        {
            _context.Student.Add(model);
            _context.SaveChanges();
        }

        public int SaveAndGetId(Student model)
        {
            _context.Student.Add(model);
            var IsSave = _context.SaveChanges();
            return model.Id;
        }

        public void Update(Student model)
        {
            _context.Student.Update(model);
            _context.SaveChanges();
        }

        public bool PromotStudent(Student model)
        {
            var student = _context.Student.FirstOrDefault(x => x.Id == model.Id);

            if (student != null)
            {
                student.Class = model.Class;
                student.Course = model.Course;
                student.Year = model.Year;
                student.Session = model.Session;
                student.AdmissionDate = model.AdmissionDate;
                student.AdmissionFormNo = model.AdmissionFormNo;
                student.MobileNoOne = model.MobileNoOne;
               var status  =  _context.SaveChanges();
                return true;
            }
            return false;
        }


        public bool UpdatePersonalDetail(Student model)
        {
            var student = _context.Student.FirstOrDefault(x => x.Id == model.Id);

            if (student != null)
            {
                student.StudentName = model.StudentName;
                student.FatherName = model.FatherName;
                student.MotherName = model.MotherName;
                student.MobileNoOne = model.MobileNoOne;
                student.DOB = model.DOB;
                student.FatherMobileNo = model.FatherMobileNo;
                student.Gender = model.Gender;
                student.Minority = model.Minority;
                student.Caste = model.Caste;
                student.AadhaarNo = model.AadhaarNo;
                student.AbcNo = model.AbcNo;
                student.SamagraID = model.SamagraID;
                student.Address = model.Address;
                student.TC = model.TC;
                student.PH = model.PH;
                student.UpdateDatetime = model.UpdateDatetime;
                student.UpdatedBy = model.UpdatedBy;
                var status = _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool PromoteStudentDetail(Student model)
        {
            var student = _context.Student.FirstOrDefault(x => x.Id == model.Id);

            if (student != null)
            {
                
                student.Year = model.Year;
                student.Course = model.Course;
                student.Class = model.Class;
                student.Session = model.Session;
                student.ExamFormSubmited = model.ExamFormSubmited;
                student.UpdateDatetime = model.UpdateDatetime;
                student.UpdatedBy = model.UpdatedBy;
                var status = _context.SaveChanges();
                return true;
            }
            return false;
        }


        public bool UpdateCollegeDetail(Student model)
        {
            var student = _context.Student.FirstOrDefault(x => x.Id == model.Id);

            if (student != null)
            {
                student.AdmissionFormNo = model.AdmissionFormNo;
                student.AdmissionDate = model.AdmissionDate;
                student.NewOld = model.NewOld;
                student.Medium = model.Medium;
                //student.Session = model.Session;
                //student.Class = model.Class;
                //student.Course = model.Course;
                //student.Year = model.Year;
                student.EnRollNo = model.EnRollNo;
                student.RollNo = model.RollNo;
                student.SchoolarNo = model.SchoolarNo;
                student.SubCode = model.SubCode;
                student.RegEx = model.RegEx;
                student.UpdateDatetime = model.UpdateDatetime;
                student.UpdatedBy = model.UpdatedBy;
               
                var status = _context.SaveChanges();
                return true;
            }
            return false;
        }

        public void Delete(int id)
        {
            var item = _context.Student.Find(id);
            if (item != null)
            {
                _context.Student.Remove(item);
                _context.SaveChanges();
            }
        }

        public Student GetById(int id)
        {
            //var teee = _context.Student.FirstOrDefault(x=>x.Id == id);
            var teee = _context.Student.Where(x => x.Id == id)
                      .FirstOrDefault();

            return _context.Student.Find(id);
        }

        public Student GetByAdmissionNumbr(int id)
        {
            var teee = _context.Student.Where(x => x.AdmissionFormNo == id)
                      .FirstOrDefault();

            return teee;
        }

        public List<Student> GetByStudentRegistrationPage(
                            string session,
                            string @class,
                            string course,
                            string year,
                            string studentName)
                                {
                                    IQueryable<Student> query = _context.Student;

                                    if (!string.IsNullOrWhiteSpace(studentName))
                                    {
                                        query = query.Where(x => x.StudentName.ToLower().Contains(studentName.ToLower()));
                                    }
                                    if (!string.IsNullOrWhiteSpace(session) && (session != "Select"))
                                    {
                                        query = query.Where(x => x.Session.ToLower().Contains(session.ToLower()));
                                    }
                                    if (!string.IsNullOrWhiteSpace(@class) && (@class != "Select"))
                                    {
                                        query = query.Where(x => x.Class.ToLower().Contains(@class.ToLower()));
                                    }
                                    if (!string.IsNullOrWhiteSpace(course) && (course != "Select"))
                                    {
                                        query = query.Where(x => x.Course.ToLower().Contains(course.ToLower()));
                                    }
                                    if (!string.IsNullOrWhiteSpace(year) && (year != "Select"))
                                    {
                                        query = query.Where(x => x.Year.ToLower().Contains(year.ToLower()));
                                    }

                                    return query.ToList();
                                }

        public List<Student> FilterList(string name, string classes, string course, string year, string session)
        {
            try { 
            
            IQueryable<Student> query = _context.Student;

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(x => x.StudentName.ToLower().Contains(name.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(classes) && (classes != "Select"))
            {
                query = query.Where(x => x.Class.ToLower().Contains(classes.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(course) && (course != "Select"))
            {
                query = query.Where(x => x.Course.ToLower().Contains(course.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(year) && (year != "Select"))
            {
                query = query.Where(x => x.Year.ToLower().Contains(year.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(session) && (session != "Select"))
            {
                query = query.Where(x => x.Session.ToLower().Contains(session.ToLower()));
            }
            return query.ToList();
                //return _context.StudentRegistration.ToList(); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }


        public bool ExamFormFilled(string  ids)
        {
            if (string.IsNullOrEmpty(ids)) return false;

            var idList = ids.Split(',')
                   .Select(int.Parse)
                   .ToList();

            var students = _context.Student.Where(x => idList.Contains(x.Id)).ToList();

            if (students.Any()) 
            {
                foreach (var student in students)
                {
                    student.ExamFormSubmited = "Yes"; // Replace with your actual column name
                }

                // 4. Save all changes in one database transaction
                _context.SaveChanges();
                return true;
            }

            return false;
        }


        //---------------------For Dashboard---------------

        public DashboardModelView DashboardDetail() 
        {

            DashboardModelView dashboardModelView = new DashboardModelView();

            var varStudentList = _context.Student.ToList();
            if (varStudentList != null) 
            {
                dashboardModelView.TotalStudent = varStudentList.Count();
            }


            return dashboardModelView;

        }


        //---------------Student History List--------------
        public List<Student> StudentHistoryList(string name, string classes, string course, string year, string session)
        {
            try
            {
                var query = from his in _context.StudentHistory
                            join st in _context.Student
                            on his.StudentId equals st.Id
                            select new
                            {
                                his.StudentId,
                                his.StudentHistoryId,
                                his.Session,
                                his.Year,
                                his.Classs,
                                his.Course,
                                his.AdmissionDate,
                                his.AdmissionForm,
                                st.StudentName,
                                st.FatherName,
                                st.MobileNoOne,
                                st.Gender,
                                st.Caste,
                                st.NewOld,
                                st.RegEx
                            };

                if (!string.IsNullOrWhiteSpace(name))
                {
                    query = query.Where(x => x.StudentName.ToLower().Contains(name.ToLower()));
                }
                if (!string.IsNullOrWhiteSpace(classes) && (classes != "Select"))
                {
                    query = query.Where(x => x.Classs.ToLower().Contains(classes.ToLower()));
                }
                if (!string.IsNullOrWhiteSpace(course) && (course != "Select"))
                {
                    query = query.Where(x => x.Course.ToLower().Contains(course.ToLower()));
                }
                if (!string.IsNullOrWhiteSpace(year) && (year != "Select"))
                {
                    query = query.Where(x => x.Year.ToLower().Contains(year.ToLower()));
                }
                if (!string.IsNullOrWhiteSpace(session) && (session != "Select"))
                {
                    query = query.Where(x => x.Session.ToLower().Contains(session.ToLower()));
                }
                List<Student> retunList = new List<Student>();
                foreach (var row in query) 
                {
                    Student stu = new Student();
                    stu.AdmissionDate = row.AdmissionDate;
                    stu.AdmissionFormNo = row.AdmissionForm;
                    stu.StudentName = row.StudentName;
                    stu.FatherMobileNo = row.FatherName;
                    stu.MotherName = row.MobileNoOne;
                    stu.MobileNoOne = row.MobileNoOne;
                    stu.Year = row.Year;
                    stu.Session = row.Session;
                    stu.Class = row.Classs;
                    stu.Course = row.Course;
                    stu.Caste = row.Caste;
                    stu.Gender = row.Gender;
                    retunList.Add(stu);
                }

                return retunList.ToList();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }



    }
}
