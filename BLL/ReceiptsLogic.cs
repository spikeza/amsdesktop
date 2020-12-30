using System;
using System.Collections.Generic;
using System.Globalization;
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

        public Receipt GetReceipt(long receiptId)
        {
            return new ReceiptsRepository().GetReceipt(receiptId);
        }

        public List<ReceiptDataGridView> SearchReceiptsForDataGrid(string searchValue, string searchMode, DateTime fromDate, DateTime toDate, long apartmentId)
        {
            return new ReceiptsRepository().SearchReceiptsForDataGrid(searchValue, searchMode, fromDate, toDate, apartmentId);
        }

        public string GetNewReceiptNumber(long apartmentId)
        {
            return new ReceiptsRepository().GetNewReceiptNumber(apartmentId);
        }

        public void AddReceipt(Receipt receipt)
        {
            new ReceiptsRepository().AddReceipt(receipt);
        }

        public void UpdateReceipt(Receipt receipt)
        {
            new ReceiptsRepository().UpdateReceipt(receipt);
        }

        public void DeleteReceipt(Receipt receipt)
        {
            new ReceiptsRepository().DeleteReceipt(receipt);
        }

        public List<ReceiptForPrinting> GetReceiptForPrinting(Receipt receipt)
        {
            List<ReceiptForPrinting> receipts = new List<ReceiptForPrinting>();

            receipts.Add(new ReceiptForPrinting()
            {
                ApartmentName = Global.CurrentApartment.ApartmentName,
                ApartmentAddress = Global.CurrentApartment.Address,
                ReceiptNo = receipt.ReceiptNo,
                RoomNo = receipt.Invoice.Room.RoomNo,
                MonthNo = receipt.Invoice.MonthNo,
                RcpDate = receipt.RcpDate.ToString("d MMMM yyyy", new CultureInfo("th-TH")),
                ContactName = receipt.Invoice.Room.Customer.ContactName,
                WMeterStart = receipt.Invoice.WMeterStart,
                EMeterStart = receipt.Invoice.EMeterStart,
                WUsedUnit = receipt.Invoice.WUsedUnit,
                EUsedUnit = receipt.Invoice.EUsedUnit,
                WMeterEnd = receipt.Invoice.WMeterStart + receipt.Invoice.WUsedUnit,
                EMeterEnd = receipt.Invoice.EMeterStart + receipt.Invoice.EUsedUnit,
                WUnit = receipt.Invoice.WUnit,
                EUnit = receipt.Invoice.EUnit,
                WAmount = receipt.Invoice.WUsedUnit * receipt.Invoice.WUnit,
                EAmount = receipt.Invoice.EUsedUnit * receipt.Invoice.EUnit,
                TelCost = receipt.Invoice.TelCost,
                MonthCost = receipt.Invoice.Room.MonthCost,
                ImproveText = receipt.Invoice.ImproveText,
                ImproveCost = receipt.Invoice.ImproveCost,
                Total = (receipt.Invoice.WUsedUnit * receipt.Invoice.WUnit) + (receipt.Invoice.EUsedUnit * receipt.Invoice.EUnit) + receipt.Invoice.TelCost + receipt.Invoice.Room.MonthCost + receipt.Invoice.ImproveCost,
                GrandTotal = receipt.Invoice.GrandTotal,
                GrandTotalText = receipt.Invoice.GrandTotalText,
                Comment = receipt.Comment
            });

            return receipts;
        }

        public bool IsThisMonthReceiptExists(long roomId, long month, int year)
        {
            return new ReceiptsRepository().IsThisMonthReceiptExists(roomId, month, year);
        }
    }
}
