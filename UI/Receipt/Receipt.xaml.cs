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
            dpFromDate.SelectedDate = DateTime.Now.AddMonths(-6);
            dpToDate.SelectedDate = DateTime.Now;
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

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddReceipt addReceiptWindow = new AddReceipt();
            addReceiptWindow.ShowDialog();
            dgReceipts.ItemsSource = new ReceiptsLogic().GetReceiptsForDataGrid(dpFromDate.SelectedDate.Value, dpToDate.SelectedDate.Value, Global.CurrentApartment.ApartmentId);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

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
