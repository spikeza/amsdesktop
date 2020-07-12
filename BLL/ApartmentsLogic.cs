using AMSDesktop.DAL.Model;
using AMSDesktop.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace AMSDesktop.BLL
{
    public class ApartmentsLogic
    {
        public List<Apartment> GetApartments()
        {
            return new ApartmentsRepository().GetApartments();
        }

        public Apartment GetApartment(long apartmentId)
        {
            return new ApartmentsRepository().GetApartment(apartmentId);
        }
        public void AddApartment(Apartment apartment)
        {
            new ApartmentsRepository().AddApartment(apartment);
        }

        public void UpdateApartment(Apartment apartment)
        {
            new ApartmentsRepository().UpdateApartment(apartment);
        }

        public void DaleteApartment(Apartment apartment)
        {
            new ApartmentsRepository().DeleteApartment(apartment);
        }
    }
}
