using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class StudentFee
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
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
        public int PaidAmount { get; set; }
        public int Scholership { get; set; }
        public string DisBy { get; set; }
        public string DisResion { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
