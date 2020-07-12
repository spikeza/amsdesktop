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

namespace AMSDesktop.UI.Apartment
{
    /// <summary>
    /// Interaction logic for UpdateApartment.xaml
    /// </summary>
    public partial class UpdateApartment : Window
    {
        private Model.Apartment _apartment;
        public UpdateApartment(Model.Apartment apartment)
        {
            InitializeComponent();
            _apartment = apartment;
            PopulateApartmentData(_apartment);
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _apartment.ApartmentName = tbxApartmentName.Text;
                _apartment.Address = tbxAddress.Text;
                _apartment.CompanyName = tbxCompanyName.Text;
                _apartment.TaxId = tbxTaxId.Text;
                _apartment.Tel = tbxTel.Text;
                new ApartmentsLogic().UpdateApartment(_apartment);
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

        private void PopulateApartmentData(Model.Apartment apartment)
        {
            tbxApartmentName.Text = apartment.ApartmentName;
            tbxAddress.Text = apartment.Address;
            tbxCompanyName.Text = apartment.CompanyName;
            tbxTaxId.Text = apartment.TaxId;
            tbxTel.Text = apartment.Tel;
        }
    }
}
