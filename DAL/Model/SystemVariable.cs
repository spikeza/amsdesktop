using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMSDesktop.DAL.Model
{
    public class SystemVariable
    {
        public long ApartmentId { get; set; }
        public string OwnerName { get; set; }
        public string CardId { get; set; }
        public string BuildingName { get; set; }
        public string OwnerAddress { get; set; }
        public Single WUnit { get; set; }
        public Single EUnit { get; set; }
        public bool IncWUnit { get; set; }
        public bool IncEUnit { get; set; }
        public bool IncTUnit { get; set; }
        public bool IncImprove { get; set; }
        public string StartInv { get; set; }
        public string EndPay { get; set; }
        public Decimal InterestRate { get; set; }
        public bool IncInterest { get; set; }
        public Single VatAmount { get; set; }
        public string TaxId { get; set; }
        public bool Paid { get; set; }
        public string PaperSize { get; set; }
        public string HeadInvoice { get; set; }
        public string HeadReciept { get; set; }
        public bool IncFrame { get; set; }
    }
}
