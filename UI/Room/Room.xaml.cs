using AMSDesktop.BLL;
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
            
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

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
