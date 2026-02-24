using CRM.Models;
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
        


    }
}
