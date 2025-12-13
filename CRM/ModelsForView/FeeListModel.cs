using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.ModelsForView
{
    public class FeeListModel
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

        //--------------Subject------------

        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string SubjectCourse { get; set; }
        public string SubjectClass { get; set; }
    }
}
