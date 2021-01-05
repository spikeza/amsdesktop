using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMSDesktop.DAL.Model
{
    public class IncomeReportRecord
    {
        public string RoomNo { get; set; }
        public string ContactName { get; set; }
        public string MonthName { get; set; }
        public Decimal ElectricCost { get; set; }
        public Decimal WaterCost { get; set; }
        public Decimal TelephoneCost { get; set; }
        public Decimal MonthCost { get; set; }
        public Decimal ImproveCost { get; set; }
        public Decimal Total { get; set; }
    }
}
