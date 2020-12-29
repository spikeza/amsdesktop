using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMSDesktop.DAL.Model
{
    public class ReceiptForPrinting
    {
        public string ApartmentName { get; set; }
        public string ApartmentAddress { get; set; }
        public string ReceiptNo { get; set; }
        public string RoomNo { get; set; }
        public long MonthNo { get; set; }
        public string RcpDate { get; set; }
        public string ContactName { get; set; }
        public long WMeterStart { get; set; }
        public long EMeterStart { get; set; }
        public long WMeterEnd { get; set; }
        public long EMeterEnd { get; set; }
        public long WUsedUnit { get; set; }
        public long EUsedUnit { get; set; }
        public Decimal WAmount { get; set; }
        public Decimal EAmount { get; set; }
        public Decimal TelCost { get; set; }
        public Decimal WUnit { get; set; }
        public Decimal EUnit { get; set; }
        public Decimal MonthCost { get; set; }
        public string ImproveText { get; set; }
        public Decimal ImproveCost { get; set; }
        public string Comment { get; set; }
        public Decimal Total { get; set; }
        public Single GrandTotal { get; set; }
        public string GrandTotalText { get; set; }
    }
}
