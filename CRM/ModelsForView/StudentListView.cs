using CRM.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.ModelsForView
{
    public class StudentListView
    {
        public List<StudentRegistration> StudentList { get; set; }

        public List<SelectListItem> ClassList { get; set; }
        public List<SelectListItem> CourseList { get; set; }

        public List<SelectListItem> YearList { get; set; }

        public List<SelectListItem> SubjectList { get; set; }

    }
}
