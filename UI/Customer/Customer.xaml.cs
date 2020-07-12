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
using AMSDesktop.DAL.Repository;
using Model = AMSDesktop.DAL.Model;
using AMSDesktop.BLL;

namespace AMSDesktop.UI.Customer
{
    /// <summary>
    /// Interaction logic for Customer.xaml
    /// </summary>
    public partial class Customer : Window
    {
        public Customer()
        {
            InitializeComponent();
            dgCustomers.ItemsSource = new CustomersRepository().GetCustomers();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            dgCustomers.ItemsSource = new CustomersRepository().SearchCustomers(tbxSearchValue.Text);
        }

        private void tbxSearchValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                dgCustomers.ItemsSource = new CustomersRepository().SearchCustomers(tbxSearchValue.Text);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddCustomer addCustomerWindow = new AddCustomer();
            if (addCustomerWindow.ShowDialog() == true)
            {
                dgCustomers.ItemsSource = new CustomersRepository().GetCustomers();
            }
        }

        private void dgCustomers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                DataGrid grid = sender as DataGrid;
                if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                {
                    DataGridRow row = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;
                    var selectedApartment = row.Item as Model.Customer;
                    UpdateCustomerData(selectedApartment);
                }
            }
        }

        private void UpdateCustomerData(Model.Customer customer)
        {
            UpdateCustomer updateWindow = new UpdateCustomer(customer);
            if (updateWindow.ShowDialog() == true)
            {
                dgCustomers.ItemsSource = new CustomersLogic().GetCustomers();
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selectedCustomer = dgCustomers.SelectedItem as Model.Customer;
            if (selectedCustomer != null)
            {
                UpdateCustomerData(selectedCustomer);
            }
            else
            {
                MessageBox.Show("กรุณาเลือกข้อมูลที่จะแก้ไข", "เกิดข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedCustomer = dgCustomers.SelectedItem as Model.Customer;
            if (selectedCustomer != null)
            {
                if (MessageBox.Show("ยืนยันที่จะลบข้อมูลอพาร์ตเมนต์ " + selectedCustomer.ContactName, "ยืนยันการลบข้อมูล", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    CustomersLogic cl = new CustomersLogic();
                    cl.DaleteCustomer(selectedCustomer);
                    dgCustomers.ItemsSource = new CustomersLogic().GetCustomers();
                }
            }
            else
            {
                MessageBox.Show("กรุณาเลือกข้อมูลที่จะลบ", "เกิดข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
