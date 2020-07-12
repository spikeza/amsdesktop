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
using Model = AMSDesktop.DAL.Model;
using AMSDesktop.BLL;

namespace AMSDesktop.UI.Customer
{
    /// <summary>
    /// Interaction logic for UpdateCustomer.xaml
    /// </summary>
    public partial class UpdateCustomer : Window
    {
        private Model.Customer _customer;
        public UpdateCustomer(Model.Customer customer)
        {
            InitializeComponent();
            _customer = customer;
            PopulateCustomerData(_customer);
        }

        private void PopulateCustomerData(Model.Customer customer)
        {
            tbxContactName.Text = customer.ContactName;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _customer.ContactName = tbxContactName.Text;
                new CustomersLogic().UpdateCustomer(_customer);
                MessageBox.Show("การแก้ไขข้อมูลสำเร็จเรียบร้อย", "สำเร็จ", MessageBoxButton.OK, MessageBoxImage.Information);

                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "เกิดข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
