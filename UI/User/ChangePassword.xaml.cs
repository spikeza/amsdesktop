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
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Window
    {
        Model.User _selectedUser;
        public ChangePassword(Model.User user)
        {
            InitializeComponent();
            _selectedUser = user;
            tbxUsername.Text = _selectedUser.Username;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsFormInputValid())
                {
                    if (_selectedUser != null)
                    {
                        string salt = new CryptographyHelper().GetGeneratedSalt();
                        _selectedUser.Password = new CryptographyHelper().GetHashedString(pwbPassword.Password + salt);
                        _selectedUser.Salt = salt;

                        new UsersLogic().ChangePassword(_selectedUser);

                        MessageBox.Show("การแก้ไขข้อมูลสำเร็จเรียบร้อย", "สำเร็จ", MessageBoxButton.OK, MessageBoxImage.Information);

                        this.DialogResult = true;
                    }
                }
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

        private bool IsOldPasswordValid()
        {
            return _selectedUser.Password == new CryptographyHelper().GetHashedString(pwbOldPassword.Password + _selectedUser.Salt) ? true : false;
        }

        private bool IsFormInputValid()
        {
            StringBuilder errorList = new StringBuilder();
            if (pwbPassword.Password != pwbConfirmPassword.Password)
            {
                errorList.AppendLine("การยืนยันรหัสผ่านใหม่ ไม่ถูกต้อง");
            }
            if (!IsOldPasswordValid())
            {
                errorList.AppendLine("รหัสผ่านเดิมไม่ถูกต้อง");
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
