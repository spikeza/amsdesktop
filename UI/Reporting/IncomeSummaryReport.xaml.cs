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
using AMSDesktop.BLL;
using System.ComponentModel;

namespace AMSDesktop.UI.Reporting
{
    /// <summary>
    /// Interaction logic for IncomeSummaryReport.xaml
    /// </summary>
    public partial class IncomeSummaryReport : Window
    {
        CultureInfo thCulture = new CultureInfo("th-TH");
        public IncomeSummaryReport()
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
                int reportMonthNo = cbbMonth.SelectedIndex + 1;
                int reportYearNo = int.Parse(reportYear) - 543;
                List<ReportParameter> parameters = new List<ReportParameter>();
                parameters.Add(new ReportParameter("ApartmentName", Global.CurrentApartment.ApartmentName));
                parameters.Add(new ReportParameter("ApartmentAddress", Global.CurrentApartment.Address));
                parameters.Add(new ReportParameter("ReportYear", reportYear));
                parameters.Add(new ReportParameter("ReportMonth", reportMonth));
                parameters.Add(cbDeductImproveCost.IsChecked == true ? new ReportParameter("DeductImproveCost", "True") : new ReportParameter("DeductImproveCost", "False"));
                ReportPreviewer rp = new ReportPreviewer();

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += (o, ea) =>
                {
                    rp.SetDataSet("IncomeSummaryDataSet", new ReceiptsLogic().GetIncomeSummaryRecords(reportMonthNo, reportYearNo, Global.CurrentApartment.ApartmentId));
                };

                worker.RunWorkerCompleted += (o, ea) =>
                {
                    rp.SetReportPath(@".\Reports\IncomeSummary.rdlc");
                    rp.SetParameters(parameters);
                    rp.ShowDialog();
                    loadingPanel.IsBusy = false;
                };

                loadingPanel.IsBusy = true;

                worker.RunWorkerAsync();
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
