using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMSDesktop.DAL.Model
{
    public class Receipt
    {
        public long ReceiptId { get; set; }
        public Invoice Invoice { get; set; }
        public long ApartmentId { get; set; }
        public Room Room { get; set; }
        public Decimal InterestUnit { get; set; }
        public long AmountDay { get; set; }
        public DateTime RcpDate { get; set; }
        public string Comment { get; set; }
        public string TotalText { get; set; }
        public Single GrandTotal { get; set; }
        public string GrandTotalText { get; set; }
    }
}
