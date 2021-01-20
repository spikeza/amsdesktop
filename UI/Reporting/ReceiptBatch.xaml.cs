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
using GreatFriends.ThaiBahtText;
using Model = AMSDesktop.DAL.Model;
using AMSDesktop.BLL;
using System.ComponentModel;

namespace AMSDesktop.UI.Reporting
{
    /// <summary>
    /// Interaction logic for ReceiptBatch.xaml
    /// </summary>
    public partial class ReceiptBatch : Window
    {
        CultureInfo thCulture = new CultureInfo("th-TH");
        public ReceiptBatch()
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
                bool deductImproveCost = false;
                int reportMonth = cbbMonth.SelectedIndex + 1;
                int reportYear = int.Parse(cbbYear.SelectedItem.ToString());
                DateTime fromDate = new DateTime(reportYear - 543, reportMonth, 1);
                DateTime toDate = fromDate.AddMonths(1).AddDays(-1);
                List<Model.ReceiptForPrinting> printReceipts = new List<Model.ReceiptForPrinting>();
                ReportPreviewer rp = new ReportPreviewer();

                string reportPath = @".\Reports\Receipt.rdlc";

                Receipt.DeductImproveCostComfirmBox confirmBox = new Receipt.DeductImproveCostComfirmBox();
                confirmBox.WindowStartupLocation = WindowStartupLocation.Manual;
                confirmBox.Top = Mouse.GetPosition(null).Y;
                confirmBox.Left = Mouse.GetPosition(null).X;
                if (confirmBox.ShowDialog() == false)
                {
                    reportPath = @".\Reports\ReceiptDeductImproveCost.rdlc";
                    deductImproveCost = true;
                }

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += (o, ea) =>
                {
                    List<Model.Receipt> receipts = new ReceiptsLogic().GetReceipts(fromDate, toDate, Global.CurrentApartment.ApartmentId);
                    foreach (var r in receipts)
                    {
                        List<Model.ReceiptForPrinting> printedReceipt = new ReceiptsLogic().GetReceiptForPrinting(r);
                        if (deductImproveCost)
                        {
                            foreach (var pr in printedReceipt)
                            {
                                pr.GrandTotalText = ThaiBahtTextUtil.ThaiBahtText(Convert.ToDecimal(pr.GrandTotal) - pr.ImproveCost);
                            }
                        }
                        printReceipts.AddRange(printedReceipt);
                    }
                };

                worker.RunWorkerCompleted += (o, ea) =>
                {
                    rp.SetDataSet("ReceiptDataSet", printReceipts);
                    rp.SetReportPath(reportPath);
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
