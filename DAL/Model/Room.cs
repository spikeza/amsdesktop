using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMSDesktop.DAL.Model
{
    public class Room
    {
        public long RoomId { get; set; }
        public string RoomNo { get; set; }
        public long CustomerId { get; set; }
        public string ContactName { get; set; }
        public long WUnitStart { get; set; }
        public long EUnitStart { get; set; }
        public Decimal MonthCost { get; set; }
        public Decimal InsureCost { get; set; }
        public DateTime? StartDate { get; set; }
        public long ApartmentId { get; set; }
        public string Floor { get; set; }
        public string Picture { get; set; }
        public long ContractMonth { get; set; }
        public bool LandTaxedPerson { get; set; }
    }
}
