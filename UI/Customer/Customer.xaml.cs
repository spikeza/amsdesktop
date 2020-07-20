using AMSDesktop.BLL;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Model = AMSDesktop.DAL.Model;

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
            dgCustomers.ItemsSource = new CustomersLogic().GetCustomers();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            dgCustomers.ItemsSource = new CustomersLogic().SearchCustomers(tbxSearchValue.Text);
        }

        private void tbxSearchValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                dgCustomers.ItemsSource = new CustomersLogic().SearchCustomers(tbxSearchValue.Text);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddCustomer addCustomerWindow = new AddCustomer();
            if (addCustomerWindow.ShowDialog() == true)
            {
                dgCustomers.ItemsSource = new CustomersLogic().GetCustomers();
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
                    var selectedCustomer = row.Item as Model.Customer;
                    UpdateCustomerData(selectedCustomer);
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
