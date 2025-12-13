using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class MstFee
    {
        public int Id { get; set; }
        public string NewOld { get; set; }
        public string Session { get; set; }
        public string Year { get; set; }
        public string Subject { get; set; }
        public string Course { get; set; }
        public int NewStudentFee { get; set; }
        public int CMoney { get; set; }
        public int TutionFee { get; set; }
        public int OtherFee { get; set; }
        public int TotalFee { get; set; }
        public int TotalFeeCM { get; set; }

    }
}
