using CRM.Data;
using CRM.Models;
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

        public List<Student> FilterList(string name, string classes, string subject, string course, string regPvt)
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
            if (!string.IsNullOrWhiteSpace(subject) && (subject != "Select"))
            {
                query = query.Where(x => x.Course.ToLower().Contains(subject.ToLower()));
            }
            //if (!string.IsNullOrWhiteSpace(course) && (course != "Select"))
            //{
            //    query = query.Where(x => x.Course.ToLower().Contains(course.ToLower()));
            //}
            if (!string.IsNullOrWhiteSpace(regPvt) && (regPvt != "Select"))
            {
                query = query.Where(x => x.RegEx.ToLower().Contains(regPvt.ToLower()));
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

    }
}
