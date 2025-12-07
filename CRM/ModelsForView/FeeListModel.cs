using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.ModelsForView
{
    public class FeeListModel
    {

        public int Id { get; set; }
        public int FeeId { get; set; }
        public int Amount { get; set; }
        public string FeeName { get; set; }
        public string Class { get; set; }
    }
}
