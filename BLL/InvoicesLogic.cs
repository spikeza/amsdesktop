using System;
using System.Collections.Generic;
using System.Globalization;
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

        public Invoice GetInvoice(long invoiceId)
        {
            return new InvoicesRepository().GetInvoice(invoiceId);
        }

        public Invoice GetLatestInvoice(long roomId)
        {
            return new InvoicesRepository().GetLatestInvoice(roomId);
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

        public void UpdateInvoice(Invoice invoice)
        {
            new InvoicesRepository().UpdateInvoice(invoice);
        }

        public void DeleteInvoice(Invoice invoice)
        {
            new InvoicesRepository().DeleteInvoice(invoice);
        }

        public Invoice GetInvoiceForReceipt(long roomId, long month, int year)
        {
            return new InvoicesRepository().GetInvoiceForReceipt(roomId, month, year);
        }

        public List<InvoiceForPrinting> GetInvoiceForPrinting(Invoice invoice)
        {
            List<InvoiceForPrinting> invoices = new List<InvoiceForPrinting>();

            invoices.Add(new InvoiceForPrinting()
            {
                ApartmentName = Global.CurrentApartment.ApartmentName,
                ApartmentAddress = Global.CurrentApartment.Address,
                InvoiceNo = invoice.InvoiceNo,
                RoomNo = invoice.Room.RoomNo,
                MonthNo = invoice.MonthNo,
                InvDate = invoice.InvDate.ToString("d MMMM yyyy", new CultureInfo("th-TH")),
                ContactName = invoice.Room.Customer.ContactName,
                WMeterStart = invoice.WMeterStart,
                EMeterStart = invoice.EMeterStart,
                WUsedUnit = invoice.WUsedUnit,
                EUsedUnit = invoice.EUsedUnit,
                WMeterEnd = invoice.WMeterStart + invoice.WUsedUnit,
                EMeterEnd = invoice.EMeterStart + invoice.EUsedUnit,
                WUnit = invoice.WUnit,
                EUnit = invoice.EUnit,
                WAmount = invoice.WUsedUnit * invoice.WUnit,
                EAmount = invoice.EUsedUnit * invoice.EUnit,
                TelCost = invoice.TelCost,
                MonthCost = invoice.Room.MonthCost,
                ImproveText = invoice.ImproveText,
                ImproveCost = invoice.ImproveCost,
                Total = (invoice.WUsedUnit * invoice.WUnit) + (invoice.EUsedUnit * invoice.EUnit) + invoice.TelCost + invoice.Room.MonthCost + invoice.ImproveCost,
                GrandTotal = invoice.GrandTotal,
                GrandTotalText = invoice.GrandTotalText,
                Comment = invoice.Comment
            });

            return invoices;
        }

        public bool IsThisMonthInvoiceExists(long roomId, long month, int year)
        {
            return new InvoicesRepository().IsThisMonthInvoiceExists(roomId, month, year);
        }
    }
}
