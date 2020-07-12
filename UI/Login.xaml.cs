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
using AMSDesktop.BLL;
using AMSDesktop.DAL.Model;

namespace AMSDesktop.UI
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            PopulateApartmentComboBox();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            ValidateUser(tbxUsername.Text, pwbPassword.Password);
        }

        private void pwbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ValidateUser(tbxUsername.Text, pwbPassword.Password);
            }
        }

        private void ValidateUser(string username, string password)
        {
            if (cbxApartment.SelectedItem != null)
            {
                Global.CurrentApartment = new ApartmentsLogic().GetApartment(long.Parse(cbxApartment.SelectedValue.ToString()));
                Global.CurrentUser = new UsersLogic().IsAuthenticatedUser(username, password);
                if (Global.CurrentUser != null)
                {
                    this.DialogResult = true;
                }
                else
                {
                    MessageBox.Show("ชื่อผู้ใช้หรือรหัสผ่านไม่ถูกต้อง");
                }
            }
            else
            {
                MessageBox.Show("กรุณาเลือกอพาร์ตเมนต์ที่ต้องการจัดการ");
            }
            
        }

        private void wLogin_Loaded(object sender, RoutedEventArgs e)
        {
            tbxUsername.Focus();
        }

        private void PopulateApartmentComboBox()
        {
            ApartmentsLogic al = new ApartmentsLogic();
            cbxApartment.ItemsSource = al.GetApartments();
            cbxApartment.DisplayMemberPath = "ApartmentName";
            cbxApartment.SelectedValuePath = "ApartmentId";
            cbxApartment.SelectedValue = 1;
        }
    }
}
