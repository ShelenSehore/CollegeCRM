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
        public MstFee feeDetail { get; set; }
    }
}
