using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMSDesktop.DAL.Model;
using AMSDesktop.DAL.Repository;

namespace AMSDesktop.BLL
{
    public class ReceiptsLogic
    {
        public List<ReceiptDataGridView> GetReceiptsForDataGrid(DateTime fromDate, DateTime toDate, long apartmentId)
        {
            return new ReceiptsRepository().GetReceiptsForDataGrid(fromDate, toDate, apartmentId);
        }

        public List<ReceiptDataGridView> SearchReceiptsForDataGrid(string searchValue, string searchMode, DateTime fromDate, DateTime toDate, long apartmentId)
        {
            return new ReceiptsRepository().SearchReceiptsForDataGrid(searchValue, searchMode, fromDate, toDate, apartmentId);
        }
    }
}
