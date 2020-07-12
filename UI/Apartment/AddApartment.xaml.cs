using AMSDesktop.BLL;
using Model = AMSDesktop.DAL.Model;
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

namespace AMSDesktop.UI.Apartment
{
    /// <summary>
    /// Interaction logic for AddApartment.xaml
    /// </summary>
    public partial class AddApartment : Window
    {
        public AddApartment()
        {
            InitializeComponent();
            tbxApartmentName.Focus();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Model.Apartment apartment = new Model.Apartment()
                {
                    ApartmentName = tbxApartmentName.Text,
                    Address = tbxAddress.Text,
                    CompanyName = tbxCompanyName.Text,
                    TaxId = tbxTaxId.Text,
                    Tel = tbxTel.Text
                };
                new ApartmentsLogic().AddApartment(apartment);
                MessageBox.Show("การเพิ่มข้อมูลสำเร็จเรียบร้อย", "สำเร็จ", MessageBoxButton.OK, MessageBoxImage.Information);
                
                ClearForm();

                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "เกิดข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearForm()
        {
            tbxApartmentName.Clear();
            tbxAddress.Clear();
            tbxCompanyName.Clear();
            tbxTaxId.Clear();
            tbxTel.Clear();
        }
    }
}
