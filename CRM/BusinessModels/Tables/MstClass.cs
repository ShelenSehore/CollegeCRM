using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.BusinessModels.Tables
{
    public class MstClass
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string CreateBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreateDatetime { get; set; }
        public DateTime? UpdateDatetime { get; set; }
    }
}
