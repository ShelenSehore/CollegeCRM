using CRM.Data;
using CRM.Models;
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
        public void Add(StudentRegistration model)
        {
            _context.StudentRegistration.Add(model);
            _context.SaveChanges();
        }

        public void Update(StudentRegistration model)
        {
            _context.StudentRegistration.Update(model);
            _context.SaveChanges();
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

        public List<StudentRegistration> FilterList(string name, string classes, string subject, string course, string regPvt)
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
            if (!string.IsNullOrWhiteSpace(subject) && (subject != "Select"))
            {
                query = query.Where(x => x.Subject.ToLower().Contains(subject.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(course) && (course != "Select"))
            {
                query = query.Where(x => x.Course.ToLower().Contains(course.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(regPvt) && (regPvt != "Select"))
            {
                query = query.Where(x => x.RegPvt.ToLower().Contains(regPvt.ToLower()));
            }
            return query.ToList();
            //return _context.StudentRegistration.ToList(); 
        }

    }
}
