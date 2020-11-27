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
using GreatFriends.ThaiBahtText;
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
        long _waterStart;
        long _waterEnd;
        long _waterUnit;
        Decimal _waterUnitPrices;
        Decimal _waterAmount;
        long _electricStart;
        long _electricEnd;
        long _electricUnit;
        Decimal _electricUnitPrices;
        Decimal _electricAmount;
        Decimal _telephoneAmount;
        Decimal _monthCost;
        Decimal _improveCost;
        Decimal _totalAmount;
        Decimal _vatAmount;
        Decimal _grandTotalAmount;
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
            tbxTelephoneAmount.Text = "0.00";
            tbxImproveCost.Text = "0.00";
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
            if (cbbRoomNo.SelectedValue != null)
            {
                _selectedRoom = new RoomsLogic().GetRoom((long)cbbRoomNo.SelectedValue);
                PopulateFieldsOnRoomSelect();
            }
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
            try
            {
                Model.Invoice invoice = new Model.Invoice()
                {
                    ApartmentId = Global.CurrentApartment.ApartmentId,
                    InvoiceNo = tbxInvoiceNo.Text,
                    Room = _selectedRoom,
                    MonthNo = long.Parse(tbxMonth.Text),
                    InvDate = DateTime.Now.Date,
                    WMeterStart = _waterStart,
                    EMeterStart = _electricStart,
                    WUsedUnit = _waterUnit,
                    EUsedUnit = _electricUnit,
                    TelCost = _telephoneAmount,
                    WUnit = _waterUnit,
                    EUnit = _electricUnit,
                    ImproveText = ThaiBahtTextUtil.ThaiBahtText(_improveCost),
                    ImproveCost = _improveCost,
                    Comment = tbxComment.Text,
                    Paid = false,
                    TotalText = ThaiBahtTextUtil.ThaiBahtText(_totalAmount),
                    GrandTotal = Decimal.ToSingle(_grandTotalAmount),
                    GrandTotalText = ThaiBahtTextUtil.ThaiBahtText(_grandTotalAmount)
                };
                new InvoicesLogic().AddInvoice(invoice);
                MessageBox.Show("การเพิ่มข้อมูลสำเร็จเรียบร้อย", "สำเร็จ", MessageBoxButton.OK, MessageBoxImage.Information);

                ClearForm();

                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "เกิดข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
            _waterStart = tbxWaterStart.Text != "" ? long.Parse(tbxWaterStart.Text) : 0;
            _waterEnd = tbxWaterEnd.Text != "" ? long.Parse(tbxWaterEnd.Text) : 0;
            _waterUnit = _waterEnd - _waterStart;
            _waterUnitPrices = tbxWaterUnitPrices.Text != "" ? Decimal.Parse(tbxWaterUnitPrices.Text) : 0;
            _waterAmount = _waterUnit * _waterUnitPrices;
            tbxWaterUnits.Text = _waterUnit.ToString();
            tbxWaterUnitPrices.Text = _waterUnitPrices.ToString("N2", thCulture);
            tbxWaterAmount.Text = _waterAmount.ToString("N2", thCulture);

            _electricStart = tbxElectricStart.Text != "" ? long.Parse(tbxElectricStart.Text) : 0;
            _electricEnd = tbxElectricEnd.Text != "" ? long.Parse(tbxElectricEnd.Text) : 0;
            _electricUnit = _electricEnd - _electricStart;
            _electricUnitPrices = tbxElectricUnitPrices.Text != "" ? Decimal.Parse(tbxElectricUnitPrices.Text) : 0;
            _electricAmount = _electricUnit * _electricUnitPrices;
            tbxElectricUnits.Text = _electricUnit.ToString();
            tbxElectricUnitPrices.Text = _electricUnitPrices.ToString("N2", thCulture);
            tbxElectricAmount.Text = _electricAmount.ToString("N2", thCulture);

            _telephoneAmount = tbxTelephoneAmount.Text != "" ? Decimal.Parse(tbxTelephoneAmount.Text) : 0;
            _monthCost = tbxMonthCost.Text != "" ? Decimal.Parse(tbxMonthCost.Text) : 0;
            _improveCost = tbxImproveCost.Text != "" ? Decimal.Parse(tbxImproveCost.Text) : 0;

            _totalAmount = _waterAmount + _electricAmount + _telephoneAmount + _monthCost + _improveCost;
            _vatAmount = tbxVATAmount.Text != "" ? Decimal.Parse(tbxVATAmount.Text) : 0;

            tbxTotal.Text = _totalAmount.ToString("N2", thCulture);
            _grandTotalAmount = _totalAmount + _vatAmount;

            tbxGrandTotal.Text = _grandTotalAmount.ToString("N2", thCulture);
            lblGrandTotalText.Content = ThaiBahtTextUtil.ThaiBahtText(_grandTotalAmount);
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

        private void ClearForm()
        {
            _selectedRoom = null;
            _waterStart = 0;
            _waterEnd = 0;
            _waterUnit = 0;
            _waterUnitPrices = 0;
            _waterAmount = 0;
            _electricStart = 0;
            _electricEnd = 0;
            _electricUnit = 0;
            _electricUnitPrices = 0;
            _electricAmount = 0;
            _telephoneAmount = 0;
            _monthCost = 0;
            _improveCost = 0;
            _totalAmount = 0;
            _vatAmount = 0;
            _grandTotalAmount = 0;
            tbxInvoiceNo.Text = "";
            cbbRoomNo.SelectedItem = null;
            tbxMonth.Text = "";
            tbxWaterStart.Text = "";
            tbxWaterEnd.Text = "";
            tbxWaterUnits.Text = "";
            tbxWaterUnitPrices.Text = "";
            tbxWaterAmount.Text = "";
            tbxElectricStart.Text = "";
            tbxElectricEnd.Text = "";
            tbxElectricUnits.Text = "";
            tbxElectricUnitPrices.Text = "";
            tbxElectricAmount.Text = "";
            tbxTelephoneAmount.Text = "";
            tbxMonthCost.Text = "";
            tbxImproveCost.Text = "";
            tbxImproveText.Text = "ค่าใช้จ่ายอื่น ๆ";
            tbxTotal.Text = "";
            tbxVATAmount.Text = "";
            tbxGrandTotal.Text = "";
            lblGrandTotalText.Content = "";
            tbxComment.Text = "";
            PopulateFieldsOnLoad();
        }
    }
}
