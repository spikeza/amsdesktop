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
using AMSDesktop.Helpers;

namespace AMSDesktop.UI.User
{
    /// <summary>
    /// Interaction logic for AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        public AddUser()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            tbxUsername.Text = "";
            tbxFirstname.Text = "";
            tbxLastname.Text = "";
            pwbPassword.Password = "";
            pwbConfirmPassword.Password = "";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (IsFormInputValid())
            {
                try
                {
                    string generatedSalt = new CryptographyHelper().GetGeneratedSalt();
                    string password = new CryptographyHelper().GetHashedString(pwbPassword.Password + generatedSalt);
                    Model.User user = new Model.User()
                    {
                        Username = tbxUsername.Text,
                        Firstname = tbxFirstname.Text,
                        Lastname = tbxLastname.Text,
                        Password = password,
                        Salt = generatedSalt
                    };
                    new UsersLogic().AddUser(user);
                    MessageBox.Show("การเพิ่มข้อมูลสำเร็จเรียบร้อย", "สำเร็จ", MessageBoxButton.OK, MessageBoxImage.Information);

                    ClearForm();

                    this.DialogResult = true;
                }
                catch (Exception ex)
                { 
                    MessageBox.Show(ex.Message, "เกิดข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool IsFormInputValid()
        {
            StringBuilder errorList = new StringBuilder();
            if (tbxUsername.Text == "" || tbxFirstname.Text == "" || tbxLastname.Text == "" || pwbPassword.Password == "" || pwbConfirmPassword.Password == "")
            {
                errorList.AppendLine("ข้อมูลที่จำเป็นไม่ครบถ้วน");
            }
            if (pwbPassword.Password != pwbConfirmPassword.Password)
            {
                errorList.AppendLine("การยืนยันรหัสผ่านไม่ตรงกัน");
            }

            if (errorList.ToString() != "")
            {
                MessageBox.Show(errorList.ToString(), "เกิดข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
    }
}
