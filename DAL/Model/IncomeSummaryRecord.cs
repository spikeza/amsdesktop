using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMSDesktop.DAL.Model
{
    public class IncomeSummaryRecord
    {
        public string ReceiptNo { get; set; }
        public string InvoiceNo { get; set; }
        public string RcpDate { get; set; }
        public Decimal ImproveCost { get; set; }
        public Decimal GrandTotal { get; set; }
    }
}
