using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.ModelsForView
{
    public class StudentRegistrationForView
    {
        public int Id { get; set; }
        public int RegFeeId { get; set; }
        public int? RegNo { get; set; }

        [Required(ErrorMessage = "Enter form number")]
        public int? FormNo { get; set; }
        public int? SchoNo { get; set; }

        [Required(ErrorMessage = "Please select session")]
        public string? Session { get; set; }

        [Required(ErrorMessage = "Please select Year")]
        public string? Year { get; set; }

        
        public string? Subject { get; set; }

        [Required(ErrorMessage = "Please select Course")]
        public string? Course { get; set; }

        [Required(ErrorMessage = "Please select Class")]
        public string? Class { get; set; }
        
        public string? Sem { get; set; }

        [Required(ErrorMessage = "Please select RegPvt")]
        public string? RegPvt { get; set; }

        [Required(ErrorMessage = "Please select Status")]
        public string? Status { get; set; }

        [Required(ErrorMessage = "Student Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Father Name is required")]
        public string? FatherName { get; set; }

        [Required(ErrorMessage = "Mother Name is required")]
        public string? MotherName { get; set; }

        [Required(ErrorMessage = "DOB Name is required")]
        public DateTime? DOB { get; set; }

        [Required(ErrorMessage = "Select Caste")]
        public string? Caste { get; set; }

        [Required(ErrorMessage = "Select Gender")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Enter valid 10 digit mobile number")]
        public string? MobileNo { get; set; }
        public int Scholership { get; set; } = 0;
        public string? DisBy { get; set; }
        public int TotalFeeAfterDiscount { get; set; } = 0;
        
        public string? DisResion { get; set; }
        public string? CreateBy { get; set; }
        public string? CreateDate { get; set; }
        public string? UpdateBy { get; set; }
        public string? UpdateDate { get; set; }
       

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

        //-----------------Fee Detail-----
        public int NewStudentFee { get; set; } = 00;
        public int CMoney { get; set; } = 00;
        public int TutionFee { get; set; } = 00;
        public int OtherFee { get; set; } = 00;
        public int TotalFee { get; set; } = 00;
        public int TotalFeeCM { get; set; } = 00;
    }
}
