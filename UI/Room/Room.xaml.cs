using AMSDesktop.BLL;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Model = AMSDesktop.DAL.Model;

namespace AMSDesktop.UI.Room
{
    /// <summary>
    /// Interaction logic for Room.xaml
    /// </summary>
    public partial class Room : Window
    {
        public Room()
        {
            InitializeComponent();
            dgRooms.ItemsSource = new RoomsLogic().GetRooms(Global.CurrentApartment.ApartmentId);
        }

        private void dgRooms_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                DataGrid grid = sender as DataGrid;
                if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                {
                    DataGridRow row = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;
                    var selectedRoom = row.Item as Model.Room;
                    UpdateRoomData(selectedRoom);
                }
            }
        }

        private void UpdateRoomData(Model.Room room)
        {
            UpdateRoom updateWindow = new UpdateRoom(room);
            if (updateWindow.ShowDialog() == true)
            {
                dgRooms.ItemsSource = new RoomsLogic().GetRooms(Global.CurrentApartment.ApartmentId);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddRoom addRoomWindow = new AddRoom();
            if (addRoomWindow.ShowDialog() == true)
            {
                dgRooms.ItemsSource = new RoomsLogic().GetRooms(Global.CurrentApartment.ApartmentId);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selectedRoom = dgRooms.SelectedItem != null ? dgRooms.SelectedItem as Model.Room : null;
            if (selectedRoom != null)
            {
                UpdateRoomData(selectedRoom);
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
                var selectedRoom = dgRooms.SelectedItem != null ? dgRooms.SelectedItem as Model.Room : null;
                if (selectedRoom != null)
                {
                    if (MessageBox.Show("ยืนยันที่จะลบข้อมูลห้องพัก เลขที่ห้อง " + selectedRoom.RoomNo, "ยืนยันการลบข้อมูล", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                    {
                        RoomsLogic r = new RoomsLogic();
                        r.DaleteRoom(selectedRoom);
                        dgRooms.ItemsSource = new RoomsLogic().GetRooms(Global.CurrentApartment.ApartmentId);
                    }
                }
                else
                {
                    MessageBox.Show("กรุณาเลือกข้อมูลที่จะลบ", "เกิดข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "เกิดข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            dgRooms.ItemsSource = new RoomsLogic().SearchRooms(tbxSearchValue.Text, Global.CurrentApartment.ApartmentId);
        }

        private void tbxSearchValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                dgRooms.ItemsSource = new RoomsLogic().SearchRooms(tbxSearchValue.Text, Global.CurrentApartment.ApartmentId);
            }
        }
    }
}
