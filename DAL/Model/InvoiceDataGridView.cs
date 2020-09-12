using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMSDesktop.DAL.Model
{
    public class InvoiceDataGridView
    {
        public long InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string RoomNo { get; set; }
        public long MonthNo { get; set; }
        public DateTime InvDate { get; set; }
        public Single GrandTotal { get; set; }
    }
}
