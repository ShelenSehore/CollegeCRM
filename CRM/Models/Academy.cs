using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class Academy
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int RegStudentId { get; set; }
        public string SchoolName { get; set; }
        public string PassingYear { get; set; }
        public string Board { get; set; }
        public string MaxMark { get; set; }
        public string ObtMark { get; set; }
        public string Result { get; set; }
        public string Parcent { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

       
    }
}
