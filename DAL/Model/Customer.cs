using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMSDesktop.DAL.Model
{
    public class Customer
    {
        public long CustomerId { get; set; }
        public string CustomerNo { get; set; }
        public string CompanyName { get; set; }
        public string CardId { get; set; }
        public string ContactName { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public DateTime? ContactDate { get; set; }
    }
}
