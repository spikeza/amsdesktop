using AMSDesktop.BLL;
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

namespace AMSDesktop.UI.Customer
{
    /// <summary>
    /// Interaction logic for AddCustomer.xaml
    /// </summary>
    public partial class AddCustomer : Window
    {
        public AddCustomer()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Model.Customer customer = new Model.Customer()
                {
                    //Only ContactName is currently used for adding new Customer
                    ContactName = tbxContactName.Text,
                    //Other information is set as default value
                    CustomerNo = " ",
                    CompanyName = Global.CurrentApartment.CompanyName,
                    CardId = "_-____-_____-__-_",
                    Address = Global.CurrentApartment.Address,
                    Tel = " "
                };
                new CustomersLogic().AddCustomer(customer);
                MessageBox.Show("การเพิ่มข้อมูลสำเร็จเรียบร้อย", "สำเร็จ", MessageBoxButton.OK, MessageBoxImage.Information);

                ClearForm();

                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "เกิดข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ClearForm()
        {
            tbxContactName.Clear();
        }
    }
}
