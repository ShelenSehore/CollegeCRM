using CRM.Data;
using CRM.Models;
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

        public int Add(IssueAdmissionForm model)
        {
            _context.IssueAdmissionForm.Add(model);
           var chekc =   _context.SaveChanges();
            return chekc;
        }

    }
}
