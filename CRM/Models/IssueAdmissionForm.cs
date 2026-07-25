using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class IssueAdmissionForm
    {

        public int Id { get; set; }
        public int StudentId { get; set; }

        public string Session { get; set; }
        public string Year { get; set; }

        public string Class { get; set; }
        public string Course { get; set; }

        public long FormNo { get; set; }
        public string Status { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
