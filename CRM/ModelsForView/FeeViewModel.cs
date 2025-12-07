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
        public int FeeId { get; set; }
        public int Amount { get; set; }
        public string FeeName { get; set; }
        public string Class { get; set; }

        public List<SelectListItem> ClassList { get; set; }
        public string SelectedClass { get; set; }

        public List<SelectListItem> FeeList { get; set; }
        public string SelectedFee { get; set; }

        public List<FeeListModel> FeeClassList { get; set; }
        


    }
}
