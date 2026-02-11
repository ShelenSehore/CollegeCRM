using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.ModelsForView
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        public int? AdmissionFormNo { get; set; }
        public string? Year { get; set; }
        public string? Session { get; set; }
        public string? EnRollNo { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public string? Class { get; set; }
        public string? Course { get; set; }
        public string? Subject { get; set; }
        public string? RollNo { get; set; }
        public string? RegEx { get; set; }
       
        public string? SchoolarNo { get; set; }
        public string? NewOld { get; set; }
        public string? SubCode { get; set; }
        public string? Medium { get; set; }
        public string? Gender { get; set; }
        public string? Caste { get; set; }
        public string? AadhaarNo { get; set; }
        public string? StudentName { get; set; }
        public DateTime? DOB { get; set; }
        public string? SamagraID { get; set; }
        public string? FatherName { get; set; }
        public string? MobileNoOne { get; set; }
        public string? MobileNoTwo { get; set; }
        public string? MotherName { get; set; }
        public string? TC { get; set; }
        public string? PH { get; set; }
        public string? FatherMobileNo { get; set; }
        public string? Address { get; set; }
        public string? Minority { get; set; }
        public string SelectedClass { get; set; }
        public string SelectedCourse { get; set; }
        public string SelectedSubject { get; set; }
        public List<SelectListItem> ClassList { get; set; }
        public List<SelectListItem> CourseList { get; set; }

        public List<SelectListItem> YearList { get; set; }
        public List<SelectListItem> SessionList { get; set; }
        public List<SelectListItem> SubjectList { get; set; }

        //--------------Academic Detail---------------
        public string SchoolName { get; set; }
        public string PassingYear { get; set; }
        public string Board { get; set; }
        public string MaxMark { get; set; }
        public string ObtMark { get; set; }
        public string Result { get; set; }
        public string Parcent { get; set; }

        //----------------Search---------------



    }
}
