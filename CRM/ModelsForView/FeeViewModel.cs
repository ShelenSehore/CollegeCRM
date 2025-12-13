using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.ModelsForView
{
    public class FeeViewModel
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


        public List<SelectListItem> ClassList { get; set; }
        public string SelectedClass { get; set; }

        public List<SelectListItem> SubjectList { get; set; }
        public string SelectedSubject { get; set; }

        public List<SelectListItem> FeeList { get; set; }
        public string SelectedFee { get; set; }

        public List<SelectListItem> CourseList { get; set; }  //------Not using now--------
        public string SelectedCourse { get; set; }



        public List<FeeListModel> FeeClassList { get; set; }
        


    }
}
