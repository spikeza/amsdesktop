using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
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
    /// Interaction logic for IncomeReport.xaml
    /// </summary>
    public partial class IncomeReport : Window
    {
        CultureInfo thCulture = new CultureInfo("th-TH");
        public IncomeReport()
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
                string reportMonth = cbbMonth.SelectedItem.ToString();
                string reportYear = cbbYear.SelectedItem.ToString();
                List<Model.IncomeReportRecord> reportDataSet = new List<Model.IncomeReportRecord>();
                DateTime fromDate = new DateTime(int.Parse(reportYear) - 543, cbbMonth.SelectedIndex + 1, 1);
                DateTime toDate = fromDate.AddMonths(1).AddDays(-1);

                List<ReportParameter> parameters = new List<ReportParameter>();
                parameters.Add(new ReportParameter("ReportYear", reportYear));
                parameters.Add(new ReportParameter("ReportMonth", reportMonth));

                List<Model.Invoice> invoices = new InvoicesLogic().GetInvoices(fromDate, toDate, Global.CurrentApartment.ApartmentId);
                foreach (var invoice in invoices)
                {
                    Decimal electricCost = invoice.EUsedUnit * invoice.EUnit;
                    Decimal waterCost = invoice.WUsedUnit * invoice.WUnit;
                    Decimal total = electricCost + waterCost + invoice.TelCost + invoice.Room.MonthCost + invoice.ImproveCost;

                    Model.IncomeReportRecord rec = new Model.IncomeReportRecord()
                    {
                        RoomNo = invoice.Room.RoomNo,
                        ContactName = invoice.Room.Customer.ContactName,
                        MonthName = reportYear,
                        ElectricCost = electricCost,
                        WaterCost = waterCost,
                        TelephoneCost = invoice.TelCost,
                        MonthCost = invoice.Room.MonthCost,
                        ImproveCost = cbDeductImproveCost.IsChecked == true ? 0 : invoice.ImproveCost,
                        Total = cbDeductImproveCost.IsChecked == true ? (total - invoice.ImproveCost) : total
                    };

                    reportDataSet.Add(rec);
                }

                ReportPreviewer rp = new ReportPreviewer();
                rp.SetDataSet("IncomeReportDataSet", reportDataSet);
                rp.SetReportPath(@".\Reports\IncomeReport.rdlc");
                rp.SetParameters(parameters);
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
