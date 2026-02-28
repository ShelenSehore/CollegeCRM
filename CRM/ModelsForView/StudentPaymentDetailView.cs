using CRM.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.ModelsForView
{
    public class StudentPaymentDetailView
    {
        public Student studentDetail { get; set; }
        public StudentFee studentFeeDetail { get; set; }

        public List<StudentFee> FeeDueList{ get; set; }

        public MstFee feeMasterDetail { get; set; }

        public int OldDuesAmount { get; set; } = 0;

        public List<StudentTransaction> studentTransaction { get; set; }


        public List<SelectListItem> ClassList { get; set; }
        public List<SelectListItem> CourseList { get; set; }

        public List<SelectListItem> YearList { get; set; }
        public List<SelectListItem> SessionList { get; set; }
        public List<SelectListItem> SubjectList { get; set; }

    }
}
