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
    /// Interaction logic for User.xaml
    /// </summary>
    public partial class User : Window
    {
        public User()
        {
            InitializeComponent();
            dgUsers.ItemsSource = new UsersLogic().GetUsers();
        }

        private void dgUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                DataGrid grid = sender as DataGrid;
                if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                {
                    DataGridRow row = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;
                    var selectedUser = row.Item as Model.User;
                    UpdateUserData(selectedUser);
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddUser addWindow = new AddUser();
            addWindow.ShowDialog();
            dgUsers.ItemsSource = new UsersLogic().GetUsers();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = dgUsers.SelectedItem != null ? dgUsers.SelectedItem as Model.User : null;
            if (selectedUser != null)
            {
                UpdateUserData(selectedUser);
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
                var selectedUser = dgUsers.SelectedItem != null ? dgUsers.SelectedItem as Model.User : null;
                if (selectedUser != null)
                {
                    if (MessageBox.Show("ยืนยันที่จะลบข้อมูลผู้ใช้ระบบ " + selectedUser.Lastname, "ยืนยันการลบข้อมูล", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                    {
                        UsersLogic l = new UsersLogic();
                        l.DeleteUser(selectedUser);
                        dgUsers.ItemsSource = new UsersLogic().GetUsers();
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

        private void UpdateUserData(Model.User user)
        {
            UpdateUser updateWindow = new UpdateUser(user);
            if (updateWindow.ShowDialog() == true)
            {
                dgUsers.ItemsSource = new UsersLogic().GetUsers();
            }
        }
    }
}
