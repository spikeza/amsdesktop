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
        public List<Invoice> GetInvoices(DateTime fromDate, DateTime toDate)
        {
            return new InvoicesRepository().GetInvoices(fromDate, toDate);
        }

        public List<InvoiceDataGridView> GetInvoicesForDataGrid(DateTime fromDate, DateTime toDate)
        {
            return new InvoicesRepository().GetInvoicesForDataGrid(fromDate, toDate);
        }
    }
}
