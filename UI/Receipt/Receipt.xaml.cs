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

namespace AMSDesktop.UI.Receipt
{
    /// <summary>
    /// Interaction logic for Receipt.xaml
    /// </summary>
    public partial class Receipt : Window
    {
        private Model.Receipt _selectedReceipt;
        public Receipt()
        {
            InitializeComponent();
            dpFromDate.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dpToDate.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);
            dgReceipts.ItemsSource = new ReceiptsLogic().GetReceiptsForDataGrid(dpFromDate.SelectedDate.Value, dpToDate.SelectedDate.Value, Global.CurrentApartment.ApartmentId);
        }

        private void SearchReceipt()
        {
            if (IsDateCriteriaValid())
                dgReceipts.ItemsSource = new ReceiptsLogic().SearchReceiptsForDataGrid(tbxSearchValue.Text, GetSearchMode(), dpFromDate.SelectedDate.Value, dpToDate.SelectedDate.Value, Global.CurrentApartment.ApartmentId);
        }

        private string GetSearchMode()
        {
            return rbSearchByRoomNo.IsChecked.Value == true ? "RoomNo" : "ReceiptNo";
        }

        private void tbxSearchValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SearchReceipt();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchReceipt();
        }

        private void dgReceipts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                DataGrid grid = sender as DataGrid;
                if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                {
                    DataGridRow row = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;
                    _selectedReceipt = new ReceiptsLogic().GetReceipt((row.Item as Model.ReceiptDataGridView).ReceiptId);
                    UpdateReceiptData(_selectedReceipt);
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddReceipt addReceiptWindow = new AddReceipt();
            addReceiptWindow.ShowDialog();
            SearchReceipt();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selectedReceipt = dgReceipts.SelectedItem != null ? new ReceiptsLogic().GetReceipt((dgReceipts.SelectedItem as Model.ReceiptDataGridView).ReceiptId) : null;
            if (selectedReceipt != null)
            {
                UpdateReceiptData(selectedReceipt);
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
                var selectedReceipt = dgReceipts.SelectedItem != null ? new ReceiptsLogic().GetReceipt((dgReceipts.SelectedItem as Model.ReceiptDataGridView).ReceiptId) : null;
                if (selectedReceipt != null)
                {
                    if (MessageBox.Show("ยืนยันที่จะลบข้อมูลใบแจ้งหนี้ " + selectedReceipt.ReceiptNo, "ยืนยันการลบข้อมูล", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                    {
                        ReceiptsLogic l = new ReceiptsLogic();
                        l.DeleteReceipt(selectedReceipt);
                        selectedReceipt.Invoice.Paid = false;
                        new InvoicesLogic().SetInvoicePaidStatus(selectedReceipt.Invoice);
                        SearchReceipt();
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

        private void UpdateReceiptData(Model.Receipt receipt)
        {
            UpdateReceipt updateWindow = new UpdateReceipt(receipt);
            updateWindow.ShowDialog();
            SearchReceipt();
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
            SearchReceipt();
        }

        private void dpToDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchReceipt();
        }

        private void tbxSearchValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchReceipt();
        }
    }
}
