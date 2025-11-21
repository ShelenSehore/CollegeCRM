using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class MstCourse
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Class name is required")]
        [StringLength(100)]
        public string CourseName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDatetime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDatetime { get; set; }

    }
}
