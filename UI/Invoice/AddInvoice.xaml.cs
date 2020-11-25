using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace AMSDesktop.UI.Invoice
{
    /// <summary>
    /// Interaction logic for AddInvoice.xaml
    /// </summary>
    public partial class AddInvoice : Window
    {
        private Model.Room _selectedRoom;
        private CultureInfo thCulture = new CultureInfo("th-TH");
        public AddInvoice()
        {
            InitializeComponent();
            PopulateFieldsOnLoad();
        }

        private void PopulateFieldsOnLoad()
        {
            tbkApartmentName.Text = Global.CurrentApartment.ApartmentName;
            tbkApartmentAddress.Text = Global.CurrentApartment.Address;
            tbxInvoiceNo.Text = new InvoicesLogic().GetNewInvoiceNumber(Global.CurrentApartment.ApartmentId);
            PopulateRoomNoDropDown();
            tbxMonth.Text = DateTime.Now.Month.ToString();
            lblInvoiceDate.Content = DateTime.Now.ToString("d MMMM yyyy", thCulture);
        }

        private void PopulateRoomNoDropDown()
        {
            List<Model.RoomDropDownView> rooms = new RoomsLogic().GetRoomsForDropDownList(Global.CurrentApartment.ApartmentId);
            cbbRoomNo.ItemsSource = rooms;
            cbbRoomNo.DisplayMemberPath = "RoomNo";
            cbbRoomNo.SelectedValuePath = "RoomId";
        }

        private void cbbRoomNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedRoom = new RoomsLogic().GetRoom((long)cbbRoomNo.SelectedValue);
            PopulateFieldsOnRoomSelect();
        }

        private void PopulateFieldsOnRoomSelect()
        {
            lblContactName.Content = _selectedRoom.Customer.ContactName;
            tbxWaterStart.Text = _selectedRoom.WUnitStart.ToString();
            tbxElectricStart.Text = _selectedRoom.EUnitStart.ToString();
            tbxWaterUnitPrices.Text = Global.CurrentSystemVariable.WUnit.ToString();
            tbxElectricUnitPrices.Text = Global.CurrentSystemVariable.EUnit.ToString();
            tbxMonthCost.Text = _selectedRoom.MonthCost.ToString("N2", thCulture);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void FloatingValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            if(((TextBox)sender).Text.Contains("."))
            {
                if (((TextBox)sender).Text.Substring(((TextBox)sender).Text.IndexOf('.')).Length <= 2
                    || ((TextBox)sender).SelectionStart <= ((TextBox)sender).Text.IndexOf('.'))
                    e.Handled = regex.IsMatch(e.Text);
                else
                    e.Handled = true;
            }
            else
            {
                e.Handled = regex.IsMatch(e.Text) && e.Text != ".";
            }
        }

        private void tbxWaterEnd_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateAllAmounts();
        }

        private void tbxElectricEnd_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateAllAmounts();
        }

        private void CalculateAllAmounts()
        {
            long waterStart = tbxWaterStart.Text != "" ? long.Parse(tbxWaterStart.Text) : 0;
            long waterEnd = tbxWaterEnd.Text != "" ? long.Parse(tbxWaterEnd.Text) : 0;
            long waterUnit = waterEnd - waterStart;
            Decimal waterUnitPrices = tbxWaterUnitPrices.Text != "" ? Decimal.Parse(tbxWaterUnitPrices.Text) : 0;
            Decimal waterAmount = waterUnit * waterUnitPrices;
            tbxWaterUnits.Text = waterUnit.ToString();
            tbxWaterUnitPrices.Text = waterUnitPrices.ToString("N2", thCulture);
            tbxWaterAmount.Text = waterAmount.ToString("N2", thCulture);

            long electricStart = tbxElectricStart.Text != "" ? long.Parse(tbxElectricStart.Text) : 0;
            long electricEnd = tbxElectricEnd.Text != "" ? long.Parse(tbxElectricEnd.Text) : 0;
            long electricUnit = electricEnd - electricStart;
            Decimal electricUnitPrices = tbxElectricUnitPrices.Text != "" ? Decimal.Parse(tbxElectricUnitPrices.Text) : 0;
            Decimal electricAmount = electricUnit * electricUnitPrices;
            tbxElectricUnits.Text = electricUnit.ToString();
            tbxElectricUnitPrices.Text = electricUnitPrices.ToString("N2", thCulture);
            tbxElectricAmount.Text = electricAmount.ToString("N2", thCulture);

            Decimal telephoneAmount = tbxTelephoneAmount.Text != "" ? Decimal.Parse(tbxTelephoneAmount.Text) : 0;
            Decimal monthCost = tbxMonthCost.Text != "" ? Decimal.Parse(tbxMonthCost.Text) : 0;
            Decimal improveCost = tbxImproveCost.Text != "" ? Decimal.Parse(tbxImproveCost.Text) : 0;

            Decimal totalAmount = waterAmount + electricAmount + telephoneAmount + monthCost + improveCost;
            tbxTotal.Text = totalAmount.ToString("N2", thCulture);
        }

        private void tbxTelephoneAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateAllAmounts();
        }

        private void tbxMonthCost_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateAllAmounts();
        }

        private void tbxImproveCost_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateAllAmounts();
        }
    }
}
