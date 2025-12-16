using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class StudentRegistration
    {

        public int Id { get; set; }
        public int? RegNo { get; set; }
        public int? FormNo { get; set; }
        public int? SchoNo { get; set; }
        public string? Session { get; set; }
        public string? Year { get; set; }
        public string? Subject { get; set; }
        public string? Course { get; set; }
        public string? Sem { get; set; }
        public string? RegPvt { get; set; }
        public string? Status { get; set; }
        public string? Name { get; set; }
        public string? FatherName { get; set; }
        public string? MotherName { get; set; }
        public DateTime? DOB { get; set; }
        public string? Caste { get; set; }
        public string? Gender { get; set; }
        public string? MobileNo { get; set; }
        public string? Scholership { get; set; }
        public string? DisBy { get; set; }
        public string? DisResion { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsMove { get; set; }



    }
}
