using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMSDesktop.BLL;
using AMSDesktop.DAL.Model;
using AMSDesktop.DAL.Repository;

namespace AMSDesktop.BLL
{
    public class CustomersLogic
    {
        public List<Customer> GetCustomers()
        {
            return new CustomersRepository().GetCustomers();
        }

        public Customer GetCustomer(long customerId)
        {
            return new CustomersRepository().GetCustomer(customerId);
        }

        public Customer GetLatestCustomer()
        {
            return new CustomersRepository().GetLatestCustomer();
        }
        public void AddCustomer(Customer customer)
        {
            new CustomersRepository().AddCustomer(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            new CustomersRepository().UpdateCustomer(customer);
        }

        public void DaleteCustomer(Customer customer)
        {
            new CustomersRepository().DeleteCustomer(customer);
        }

        public List<Customer> SearchCustomers(string searchValue)
        {
            return new CustomersRepository().SearchCustomers(searchValue);
        }
    }
}
