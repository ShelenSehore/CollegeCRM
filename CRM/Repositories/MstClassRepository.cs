using CRM.Data;
using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Repositories
{
    public class MstClassRepository
    {
        private readonly CollegeContext _context;
        public MstClassRepository(CollegeContext context)
        {
            _context = context;
        }

        public List<MstClass> GetAll()
        {
            return _context.MstClass.ToList();
        }
    }
}
