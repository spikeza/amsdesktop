using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMSDesktop.DAL.Model
{
    public class Invoice
    {
        public long InvoiceId { get; set; }
        public long ApartmentId  { get; set; }
        public string InvoiceNo { get; set; }
        public Room Room { get; set; }
        public long MonthNo { get; set; }
        public DateTime InvDate { get; set; }
        public long WMeterStart { get; set; }
        public long EMeterStart { get; set; }
        public long WUsedUnit { get; set; }
        public long EUsedUnit { get; set; }
        public Decimal TelCost { get; set; }
        public Decimal WUnit { get; set; }
        public Decimal EUnit { get; set; }
        public string ImproveText { get; set; }
        public Decimal ImproveCost { get; set; }
        public string Comment { get; set; }
        public bool Paid { get; set; }
        public string TotalText { get; set; }
        public Single GrandTotal { get; set; }
        public string GrandTotalText { get; set; }
    }
}
