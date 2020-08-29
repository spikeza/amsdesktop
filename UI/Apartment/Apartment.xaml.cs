using System;
using System.Collections.Generic;
using System.Data;
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
using AMSDesktop.BLL;
using Model = AMSDesktop.DAL.Model;

namespace AMSDesktop.UI.Apartment
{
    /// <summary>
    /// Interaction logic for Apartment.xaml
    /// </summary>
    public partial class Apartment : Window
    {
        public Apartment()
        {
            InitializeComponent();
            dgApartments.ItemsSource = new ApartmentsLogic().GetApartments();
        }

        private void dgApartments_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                DataGrid grid = sender as DataGrid;
                if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                {
                    DataGridRow row = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;
                    var selectedApartment = row.Item as Model.Apartment;
                    UpdateApartmentData(selectedApartment);
                }
            }

        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selectedApartment = dgApartments.SelectedItem as Model.Apartment;
            if (selectedApartment != null)
            {
                UpdateApartmentData(selectedApartment);
            }
            else
            {
                MessageBox.Show("กรุณาเลือกข้อมูลที่จะแก้ไข", "เกิดข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateApartmentData(Model.Apartment apartment)
        {
            UpdateApartment updateWindow = new UpdateApartment(apartment);
            if (updateWindow.ShowDialog() == true)
            {
                dgApartments.ItemsSource = new ApartmentsLogic().GetApartments();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedApartment = dgApartments.SelectedItem as Model.Apartment;
                if (selectedApartment != null)
                {
                    if (MessageBox.Show("ยืนยันที่จะลบข้อมูลอพาร์ตเมนต์ " + selectedApartment.ApartmentName, "ยืนยันการลบข้อมูล", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                    {
                        ApartmentsLogic al = new ApartmentsLogic();
                        al.DaleteApartment(selectedApartment);
                        dgApartments.ItemsSource = new ApartmentsLogic().GetApartments();
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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddApartment addApartmentWindow = new AddApartment();
            if (addApartmentWindow.ShowDialog() == true)
            {
                dgApartments.ItemsSource = new ApartmentsLogic().GetApartments();
            }
            
        }
    }
}
