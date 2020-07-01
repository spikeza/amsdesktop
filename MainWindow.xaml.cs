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
using System.Windows.Navigation;
using System.Windows.Shapes;
using AMSDesktop.UI;
using AMSDesktop.DAL.Repository;
using AMSDesktop.DAL.Model;

namespace AMSDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public User CurrentUser { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Login loginWindow = new Login();
            if (loginWindow.ShowDialog() == true)
            {
                this.CurrentUser = loginWindow.CurrentUser;
            }
            this.Title = "Welcome " + CurrentUser.Firstname;
        }
    }
}
