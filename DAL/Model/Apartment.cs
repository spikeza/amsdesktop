using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMSDesktop.DAL.Model
{
    public class Apartment
    {
        public long ApartmentId { get; set; }
        public string ApartmentName { get; set; }
        public string Address { get; set; }
        public string CompanyName { get; set; }
        public string TaxId { get; set; }
        public string Tel { get; set; }
    }
}
