using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.ModelsForView
{
    public class IssueAdmissionFormViewModel
    {
        public int StudentId { get; set; }
        public long AdmissionFormNo { get; set; }
        public string Year { get; set; }
        public string Session { get; set; }
        public string Class { get; set; }
        public string Course { get; set; }
        public string StudentName { get; set; }
        public string FatherName { get; set; }
        public string MobileNo { get; set; }
        public long FormNo { get; set; }
        public int AdmissionFormId { get; set; }
        public string FormStatus { get; set; }
        public string FormYear { get; set; }
        public string FormSession { get; set; }
        public string CreatedDate { get; set; }
        
    }
}
