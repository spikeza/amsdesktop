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
using AMSDesktop.DAL.Model;

namespace AMSDesktop.UI
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public User CurrentUser { get; set; }
        public Login()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser = new UserRepository().IsAuthenticatedUser(tbxUsername.Text, pwbPassword.Password);
            if (CurrentUser != null)
            {
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Invalid Username or Password");
            }
        }
    }
}
