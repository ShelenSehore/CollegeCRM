using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class StudentHistory
    {
        public int StudentHistoryId { get; set; }
        public int StudentId { get; set; }
        public int AdmissionForm { get; set; }
        public DateTime AdmissionDate { get; set; }
        public string Session { get; set; }
        public string Classs { get; set; }
        public string Course { get; set; }
        public string Year { get; set; }  //-----------
       
        public string ScholerNo { get; set; }
        public string RegPvt { get; set; }
        public string NewOld { get; set; }
        public string Medium { get; set; }
        public string EnrolNo { get; set; }
        public string RollNo { get; set; }
        public string Gender { get; set; }
        public string Status { get; set; }
        public string StudentName { get; set; }
        public string FatherName { get; set; }
        public string FatherMobileNo { get; set; }
        public string MotherName { get; set; }
        public string PH { get; set; }
        public string Cast { get; set; }
        public string Minority { get; set; }
        
        public DateTime? DOB { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string TCIssue { get; set; }
        public string AbcId { get; set; }
        public string SamagraId { get; set; }
        public string AdharNo { get; set; }
        public string SubCode { get; set; }
        public string Photo { get; set; }
        public string ExamFormSubmited { get; set; }
        public string Remark { get; set; }  //---------------
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }





        



    }
}
