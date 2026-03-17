using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.ModelsForView
{
    public class StudentRegDownload
    {
        public int Id { get; set; }
        public int? RegNo { get; set; }
        public int? FormNo { get; set; }
        public int? SchoNo { get; set; }
        public string? Session { get; set; }
        public string? Year { get; set; }
        public string? Subject { get; set; }
        public string? Course { get; set; }
        public string? Sem { get; set; }
        public string? RegPvt { get; set; }
        public string? Status { get; set; }
        public string? Name { get; set; }
        public string? FatherName { get; set; }
        public string? MotherName { get; set; }
        public DateTime? DOB { get; set; }
        public string? Caste { get; set; }
        public string? Gender { get; set; }
        public string? MobileNo { get; set; }
       
        public string? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsMove { get; set; } = false;

        public string NewOld { get; set; }
        public int NewStudentFee { get; set; }
        public int CMoney { get; set; }
        public int TutionFee { get; set; }
        public int OtherFee { get; set; }
        public int TotalFee { get; set; }
        public int TotalFeeCM { get; set; }
        public int TotalFeeAfterDiscount { get; set; }
        public int PaidAmount { get; set; }

        public int ScholershipExcel { get; set; }
        public string DisByExcel { get; set; }
        public string DisResionExcel { get; set; }
        public string CMoneyPaidOrNot { get; set; }
    }
}
