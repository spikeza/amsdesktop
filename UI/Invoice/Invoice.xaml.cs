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

namespace AMSDesktop.UI.Invoice
{
    /// <summary>
    /// Interaction logic for Invoice.xaml
    /// </summary>
    public partial class Invoice : Window
    {
        public Invoice()
        {
            InitializeComponent();
            dpFromDate.SelectedDate = DateTime.Now.AddMonths(-6);
            dpToDate.SelectedDate = DateTime.Now;
            dgInvoices.ItemsSource = new InvoicesLogic().GetInvoicesForDataGrid(dpFromDate.SelectedDate.Value, dpToDate.SelectedDate.Value, Global.CurrentApartment.ApartmentId);
        }

        private void tbxSearchValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                dgInvoices.ItemsSource = new InvoicesLogic().SearchInvoicesForDataGrid(tbxSearchValue.Text, GetSearchMode(),dpFromDate.SelectedDate.Value, dpToDate.SelectedDate.Value, Global.CurrentApartment.ApartmentId);
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            dgInvoices.ItemsSource = new InvoicesLogic().SearchInvoicesForDataGrid(tbxSearchValue.Text, GetSearchMode(), dpFromDate.SelectedDate.Value, dpToDate.SelectedDate.Value, Global.CurrentApartment.ApartmentId);
        }

        private void dgInvoices_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddInvoice addInvoiceWindow = new AddInvoice();
            addInvoiceWindow.ShowDialog();
            dgInvoices.ItemsSource = new InvoicesLogic().GetInvoicesForDataGrid(dpFromDate.SelectedDate.Value, dpToDate.SelectedDate.Value, Global.CurrentApartment.ApartmentId);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private string GetSearchMode()
        {
            return rbSearchByRoomNo.IsChecked.Value == true ? "RoomNo" : "InvoiceNo";
        }
    }
}
