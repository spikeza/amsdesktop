using System;
using System.Collections.Generic;
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
using Model = AMSDesktop.DAL.Model;

namespace AMSDesktop.UI.Invoice
{
    /// <summary>
    /// Interaction logic for Invoice.xaml
    /// </summary>
    public partial class Invoice : Window
    {
        private Model.Invoice _selectedInvoice;
        public Invoice()
        {
            InitializeComponent();
            dpFromDate.SelectedDate = DateTime.Now.AddMonths(-6);
            dpToDate.SelectedDate = DateTime.Now;
            dgInvoices.ItemsSource = new InvoicesLogic().GetInvoicesForDataGrid(dpFromDate.SelectedDate.Value, dpToDate.SelectedDate.Value, Global.CurrentApartment.ApartmentId);
        }

        private void SearchInvoice()
        {
            if (IsDateCriteriaValid())
                dgInvoices.ItemsSource = new InvoicesLogic().SearchInvoicesForDataGrid(tbxSearchValue.Text, GetSearchMode(), dpFromDate.SelectedDate.Value, dpToDate.SelectedDate.Value, Global.CurrentApartment.ApartmentId);
        }
        private void tbxSearchValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SearchInvoice();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchInvoice();
        }

        private void dgInvoices_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                DataGrid grid = sender as DataGrid;
                if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                {
                    DataGridRow row = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;
                    _selectedInvoice = new InvoicesLogic().GetInvoice((row.Item as Model.InvoiceDataGridView).InvoiceId);
                    UpdateInvoiceData(_selectedInvoice);
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddInvoice addInvoiceWindow = new AddInvoice();
            addInvoiceWindow.ShowDialog();
            SearchInvoice();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selectedInvoice = dgInvoices.SelectedItem != null ? new InvoicesLogic().GetInvoice((dgInvoices.SelectedItem as Model.InvoiceDataGridView).InvoiceId) : null;
            if (selectedInvoice != null)
            {
                UpdateInvoiceData(selectedInvoice);
            }
            else
            {
                MessageBox.Show("กรุณาเลือกข้อมูลที่จะแก้ไข", "เกิดข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedInvoice = dgInvoices.SelectedItem != null ? new InvoicesLogic().GetInvoice((dgInvoices.SelectedItem as Model.InvoiceDataGridView).InvoiceId) : null;
                if (selectedInvoice != null)
                {
                    if (MessageBox.Show("ยืนยันที่จะลบข้อมูลใบแจ้งหนี้ " + selectedInvoice.InvoiceNo, "ยืนยันการลบข้อมูล", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                    {
                        InvoicesLogic l = new InvoicesLogic();
                        l.DeleteInvoice(selectedInvoice);

                        Model. Invoice latestInvoice = new InvoicesLogic().GetLatestInvoice(selectedInvoice.Room.RoomId);
                        if (latestInvoice != null)
                        {
                            selectedInvoice.Room.WUnitStart = latestInvoice.WMeterStart;
                            selectedInvoice.Room.EUnitStart = latestInvoice.EMeterStart;
                            new RoomsLogic().UpdateRoomMeterStart(selectedInvoice.Room);
                        }
                        
                        SearchInvoice();
                    }
                }
                else
                {
                    MessageBox.Show("กรุณาเลือกข้อมูลที่จะลบ", "เกิดข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "เกิดข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetSearchMode()
        {
            return rbSearchByRoomNo.IsChecked.Value == true ? "RoomNo" : "InvoiceNo";
        }

        private void UpdateInvoiceData(Model.Invoice invoice)
        {
            UpdateInvoice updateWindow = new UpdateInvoice(invoice);
            updateWindow.ShowDialog();
            SearchInvoice();
        }

        private bool IsDateCriteriaValid()
        {
            if (dpFromDate.SelectedDate != null && dpToDate.SelectedDate != null)
            {
                if (dpFromDate.SelectedDate.Value < dpToDate.SelectedDate.Value)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("ช่วงเวลาของการค้นหาข้อมูลไม่ถูกต้อง", "เกิดข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            return false;
        }

        private void dpFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchInvoice();
        }

        private void dpToDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchInvoice();
        }
    }
}
