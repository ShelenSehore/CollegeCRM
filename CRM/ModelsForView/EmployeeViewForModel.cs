using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.ModelsForView
{
    public class EmployeeViewForModel
    {
        
        
        public string Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string FatherName { get; set; }

        [StringLength(100)]
        public string MotherName { get; set; }

        [StringLength(100)]
        public string Designation { get; set; }

        [StringLength(100)]
        public string Subject { get; set; }

        [StringLength(50)]
        public string MobileNo { get; set; }

        [StringLength(50)]
        public string WhatsupNo { get; set; }

        [StringLength(10)]
        public string Cast { get; set; }

        [StringLength(50)]
        public string AadharNo { get; set; }

        [StringLength(50)]
        public string EmailAddress { get; set; }

        [StringLength(50)]
        public string PanNo { get; set; }

        [StringLength(50)]
        public string BankName { get; set; }

        [Column("acno")]
        [StringLength(50)]
        public string AccountNo { get; set; }

        [StringLength(50)]
        public string IFSC { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(50)]
        public string PinCode { get; set; }

        public DateTime? DOB { get; set; }

        [StringLength(50)]
        public string Department { get; set; }

        [StringLength(50)]
        public string UP { get; set; }

        [StringLength(50)]
        public string PG { get; set; }

        [StringLength(50)]
        public string BED { get; set; }

        [StringLength(50)]
        public string MED { get; set; }

        [StringLength(50)]
        public string Other1 { get; set; }

        [StringLength(50)]
        public string Other2 { get; set; }

        [StringLength(50)]
        public string Specialization { get; set; }

        [StringLength(50)]
        public string TeachingExperience { get; set; }

        [StringLength(50)]
        public string Code28Designation { get; set; }

        [StringLength(50)]
        public string NotificationNo { get; set; }

        public DateTime? Date { get; set; }

        [StringLength(50)]
        public string AppointmentorderNo { get; set; }

        public DateTime? AppointDate { get; set; }

        public DateTime? JointingDate { get; set; }

        [StringLength(50)]
        public string PayScale { get; set; }

        [StringLength(50)]
        public string Photo { get; set; }

        [StringLength(50)]
        public string ActiveUnactive { get; set; }

        [StringLength(100)]
        public string CollegeName { get; set; }
    }
}
