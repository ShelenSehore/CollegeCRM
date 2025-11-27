using CRM.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.ModelsForView
{
    public class SubjectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Course { get; set; }
        public string Class { get; set; }


        public string SelectedClass { get; set; }
        public string SelectedCourse { get; set; }
        public List<SelectListItem> ClassList { get; set; }
        public List<SelectListItem> CourseList { get; set; }
        public List<MstSubject> SubjectList { get; set; }
    }
}
