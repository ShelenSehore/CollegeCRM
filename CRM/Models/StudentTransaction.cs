using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class StudentTransaction
    {

        public int Id { get; set; }
        public int StudentId { get; set; }
        public int StudentFeeId { get; set; }
        public string Head { get; set; }
        public string RecBookNo { get; set; }
        public string RecNumber { get; set; }
        public int Amount { get; set; } = 0;
        public string PaymentMode { get; set; }
        public string TransactionNo { get; set; }
        public string CreatedBy { get; set; }
        public string CreateDateTime { get; set; }

        public string UpdatedBy { get; set; }

        public string UpdatedDateTime { get; set; }
    }
}
