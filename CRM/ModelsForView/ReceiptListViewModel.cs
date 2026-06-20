using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.ModelsForView
{
    public class ReceiptListViewModel
    {
       
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string FatherName { get; set; }

        public string Class { get; set; }
        public string Course { get; set; }
        public string Year { get; set; }

        public int StudentFeeId { get; set; }
        public string RecBookNo { get; set; }
        public string RecNumber { get; set; }
        public string PaymentMode { get; set; }
        public int TotalAmount { get; set; }
        
        public int Count { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
