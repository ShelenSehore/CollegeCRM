using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.ModelsForView
{
    public class ReportTransactionViewModel
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
        public DateTime CreateDateTime { get; set; }

        //--------------------

        public int? AdmissionFormNo { get; set; }
        public string? Year { get; set; }
        public string? Session { get; set; }

        public string? Class { get; set; }
        public string? RegEx { get; set; }
        public string? Course { get; set; }

        public string? NewOld { get; set; }

        public string? StudentName { get; set; }

        public string? FatherName { get; set; }
        public string? MobileNoOne { get; set; }

    }
}
