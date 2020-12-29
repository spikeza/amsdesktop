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
using UI = AMSDesktop.UI;
using AMSDesktop.DAL.Repository;
using Model = AMSDesktop.DAL.Model;

namespace AMSDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            UI.Login loginWindow = new UI.Login();
            if (loginWindow.ShowDialog() == true)
            {
                this.Title = "ยินดีต้อนรับ " + Global.CurrentUser.Firstname;
            }
            else
            {
                MessageBox.Show("กรุณาลงชื่อเข้าสู่ระบบเพื่อใช้งาน");
                this.Close();
            }
        }

        private void mSelectApartment_Click(object sender, RoutedEventArgs e)
        {
            UI.Apartment.SelectApartment selectApartmentWindow = new UI.Apartment.SelectApartment();
            selectApartmentWindow.ShowDialog();
        }

        private void mApartmentData_Click(object sender, RoutedEventArgs e)
        {
            UI.Apartment.Apartment apartmentWindow = new UI.Apartment.Apartment();
            apartmentWindow.ShowDialog();
        }

        private void mCustomerData_Click(object sender, RoutedEventArgs e)
        {
            UI.Customer.Customer customerWindow = new UI.Customer.Customer();
            customerWindow.ShowDialog();
        }

        private void mRoomData_Click(object sender, RoutedEventArgs e)
        {
            UI.Room.Room roomWindow = new UI.Room.Room();
            roomWindow.ShowDialog();
        }

        private void mInvoiceData_Click(object sender, RoutedEventArgs e)
        {
            UI.Invoice.Invoice invoiceWindow = new UI.Invoice.Invoice();
            invoiceWindow.ShowDialog();
        }

        private void mReceiptData_Click(object sender, RoutedEventArgs e)
        {
            UI.Receipt.Receipt receiptWindow = new UI.Receipt.Receipt();
            receiptWindow.ShowDialog();
        }
    }
}
