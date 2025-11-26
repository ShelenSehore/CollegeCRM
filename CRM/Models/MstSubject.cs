using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class MstSubject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Subject name is required")]
        [StringLength(100)]
        public string Name { get; set; }

        public string Course { get; set; }

        public string Class { get; set; }
    }
}
