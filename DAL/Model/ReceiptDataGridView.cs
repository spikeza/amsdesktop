using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMSDesktop.DAL.Model
{
    public class ReceiptDataGridView
    {
        public long ReceiptId { get; set; }
        public string ReceiptNo { get; set; }
        public string RoomNo { get; set; }
        public long MonthNo { get; set; }
        public DateTime RcpDate { get; set; }
        public string InvoiceNo { get; set; }
        public Single GrandTotal { get; set; }
    }
}
