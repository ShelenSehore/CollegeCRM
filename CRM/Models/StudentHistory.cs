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
        public string Year { get; set; }
        public string Remark { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }





        



    }
}
