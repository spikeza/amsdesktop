using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMSDesktop.DAL.Repository;
using AMSDesktop.DAL.Model;

namespace AMSDesktop.BLL
{
    public class InvoicesLogic
    {
        public List<Invoice> GetInvoices(DateTime fromDate, DateTime toDate, long apartmentId)
        {
            return new InvoicesRepository().GetInvoices(fromDate, toDate, apartmentId);
        }

        public List<InvoiceDataGridView> GetInvoicesForDataGrid(DateTime fromDate, DateTime toDate, long apartmentId)
        {
            return new InvoicesRepository().GetInvoicesForDataGrid(fromDate, toDate, apartmentId);
        }

        public List<InvoiceDataGridView> SearchInvoicesForDataGrid(string searchValue, string searchMode, DateTime fromDate, DateTime toDate, long apartmentId)
        {
            return new InvoicesRepository().SearchInvoicesForDataGrid(searchValue, searchMode, fromDate, toDate, apartmentId);
        }
        public string GetNewInvoiceNumber(long apartmentId)
        {
            return new InvoicesRepository().GetNewInvoiceNumber(apartmentId);
        }

        public void AddInvoice(Invoice invoice)
        {
            new InvoicesRepository().AddInvoice(invoice);
        }
    }
}
