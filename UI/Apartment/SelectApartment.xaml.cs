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
using AMSDesktop.BLL;

namespace AMSDesktop.UI.Apartment
{
    /// <summary>
    /// Interaction logic for SelectApartment.xaml
    /// </summary>
    public partial class SelectApartment : Window
    {
        public SelectApartment()
        {
            InitializeComponent();
            PopulateApartmentComboBox();
        }

        private void PopulateApartmentComboBox()
        {
            cbxApartment.ItemsSource = new ApartmentsLogic().GetApartments();
            cbxApartment.DisplayMemberPath = "ApartmentName";
            cbxApartment.SelectedValuePath = "ApartmentId";
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (cbxApartment.SelectedValue != null)
            {
                Global.CurrentApartment = new ApartmentsLogic().GetApartment(long.Parse(cbxApartment.SelectedValue.ToString()));

                this.DialogResult = true;
            }
        }
    }
}
