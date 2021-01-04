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

namespace AMSDesktop.UI.User
{
    /// <summary>
    /// Interaction logic for UpdateUser.xaml
    /// </summary>
    public partial class UpdateUser : Window
    {
        Model.User _selectedUser;
        public UpdateUser(Model.User user)
        {
            InitializeComponent();
            _selectedUser = user;
            PopulateUserData();
        }

        private void PopulateUserData()
        {
            if (_selectedUser != null)
            {
                tbxUsername.Text = _selectedUser.Username;
                tbxFirstname.Text = _selectedUser.Firstname;
                tbxLastname.Text = _selectedUser.Lastname;
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsFormInputValid())
                {
                    if (_selectedUser != null)
                    {
                        _selectedUser.Firstname = tbxFirstname.Text;
                        _selectedUser.Lastname = tbxLastname.Text;
                        new UsersLogic().UpdateUser(_selectedUser);

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

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            ChangePassword cpWindow = new ChangePassword(_selectedUser);
            if(cpWindow.ShowDialog() == true)
            {
                this.DialogResult = true;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private bool IsFormInputValid()
        {
            if (tbxUsername.Text == "" || tbxFirstname.Text == "" || tbxLastname.Text == "")
            {
                MessageBox.Show("ข้อมูลที่จำเป็นไม่ครบถ้วน", "เกิดข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
    }
}
