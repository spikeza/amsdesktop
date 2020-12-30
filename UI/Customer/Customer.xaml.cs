using AMSDesktop.BLL;
using System;
using System.Diagnostics.Eventing.Reader;
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
        public Model.Customer selectedCustomer;
        public bool SelectMode { get; set; }
        public Customer()
        {
            InitializeComponent();
            dgCustomers.ItemsSource = new CustomersLogic().GetCustomers();
        }

        public Customer(bool selectMode, string searchValue = "")
        {
            if (selectMode == true)
            {
                InitializeComponent();
                SelectMode = true;
                this.Title = "เลือกข้อมูลผู้เช่า";
                this.Height = this.Height - 50;
                tbxSearchValue.Text = searchValue != "" ? searchValue : "";
                if (tbxSearchValue.Text != "")
                {
                    dgCustomers.ItemsSource = new CustomersLogic().SearchCustomers(tbxSearchValue.Text);
                }
                else
                {
                    dgCustomers.ItemsSource = new CustomersLogic().GetCustomers();
                }
                btnAdd.Visibility = Visibility.Hidden;
                btnEdit.Visibility = Visibility.Hidden;
                btnDelete.Visibility = Visibility.Hidden;
            }
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
                    selectedCustomer = row.Item as Model.Customer;
                    if (!SelectMode)
                    {
                        UpdateCustomerData(selectedCustomer);
                    }
                    else
                    {
                        this.DialogResult = true;
                    }
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
            selectedCustomer = dgCustomers.SelectedItem != null ? dgCustomers.SelectedItem as Model.Customer : null;
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
            try
            {
                selectedCustomer = dgCustomers.SelectedItem != null ? dgCustomers.SelectedItem as Model.Customer : null;
                if (selectedCustomer != null)
                {
                    if (MessageBox.Show("ยืนยันที่จะลบข้อมูลผู้เช่า " + selectedCustomer.ContactName, "ยืนยันการลบข้อมูล", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "เกิดข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
