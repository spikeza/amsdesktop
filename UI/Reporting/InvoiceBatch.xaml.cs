using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Model = AMSDesktop.DAL.Model;
using AMSDesktop.BLL;

namespace AMSDesktop.UI.Reporting
{
    /// <summary>
    /// Interaction logic for InvoiceBatch.xaml
    /// </summary>
    public partial class InvoiceBatch : Window
    {
        CultureInfo thCulture = new CultureInfo("th-TH");
        public InvoiceBatch()
        {
            InitializeComponent();
            PopulateMonthComboBox();
            PopulateYearComboBox();
        }

        private void PopulateMonthComboBox()
        {
            cbbMonth.ItemsSource = thCulture.DateTimeFormat.MonthNames.Take(12).ToList();
            cbbMonth.SelectedItem = thCulture.DateTimeFormat.MonthNames[DateTime.Now.Month - 1];
        }

        private void PopulateYearComboBox()
        {
            DateTime today = DateTime.Now.Date;
            System.Globalization.Calendar thCalendar = thCulture.Calendar;
            int currentYear = thCalendar.GetYear(today);
            int dataRetentionYears = int.Parse(ConfigurationManager.AppSettings["DataRetentionYears"].ToString());
            List<int> years = new List<int>();

            for (int i = currentYear; i >= currentYear - dataRetentionYears; i--)
                years.Add(i);
            cbbYear.ItemsSource = years;
            cbbYear.SelectedItem = currentYear;
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int reportMonth = cbbMonth.SelectedIndex + 1;
                int reportYear = int.Parse(cbbYear.SelectedItem.ToString());
                DateTime fromDate = new DateTime(reportYear - 543, reportMonth, 1);
                DateTime toDate = fromDate.AddMonths(1).AddDays(-1);

                List<Model.Invoice> invoices = new InvoicesLogic().GetInvoices(fromDate, toDate, Global.CurrentApartment.ApartmentId);
                List<Model.InvoiceForPrinting> printInvoices = new List<Model.InvoiceForPrinting>();

                foreach (var inv in invoices)
                {
                    printInvoices.AddRange(new InvoicesLogic().GetInvoiceForPrinting(inv));
                }
                ReportPreviewer rp = new ReportPreviewer();
                rp.SetDataSet("InvoiceDataSet", printInvoices);
                rp.SetReportPath(@".\Reports\InvoiceBatch.rdlc");
                rp.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "เกิดข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
