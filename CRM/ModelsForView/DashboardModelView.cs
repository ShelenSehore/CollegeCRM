using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.ModelsForView
{
    public class DashboardModelView
    {
        public int TotalStudent { get; set; } = 0;
        public int TotalRegular { get; set; } = 0;
        public int TotalPrivate { get; set; } = 0;
        public int TotalNew { get; set; } = 0;

        public int Total1Year { get; set; } = 0;
        public int Total2Year { get; set; } = 0;
        public int Total3Year { get; set; } = 0;
        public int TotalCousionMoneyPaid { get; set; } = 0;

    }
}
