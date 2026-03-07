using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.ModelsForView
{
    public class ReportStudentFeeView
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string FatherName { get; set; }
        public string MobileNoOne { get; set; }
        public string Year { get; set; }
        public string Course { get; set; }
        public string Class { get; set; }
        public string Session { get; set; }
        public string NewOld { get; set; }
        public int NewStudentFee { get; set; }
        public int CMoney { get; set; }
        public int TutionFee { get; set; }
        public int OtherFee { get; set; }
        public int TotalFee { get; set; }
        public int TotalFeeCM { get; set; }
        public int TotalFeeAfterDiscount { get; set; }
        public int PaidAmount { get; set; }

        public int Scholership { get; set; }
        public string DisBy { get; set; }
        public string DisResion { get; set; }
        public string CMoneyPaidOrNot { get; set; }

    }
}
