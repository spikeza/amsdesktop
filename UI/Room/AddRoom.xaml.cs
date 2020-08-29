using System;
using System.Collections.Generic;
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
using Model = AMSDesktop.DAL.Model;
using UI = AMSDesktop.UI;
using AMSDesktop.BLL;
using System.Security.AccessControl;

namespace AMSDesktop.UI.Room
{
    /// <summary>
    /// Interaction logic for AddRoom.xaml
    /// </summary>
    public partial class AddRoom : Window
    {
        private bool _isNewCustomer = true;
        private Model.Customer _selectedCustomer;
        public AddRoom()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsFormInputsValid())
                {
                    if (_isNewCustomer)
                    {
                        Model.Customer customer = new Model.Customer()
                        {
                            ContactName = tbxContactName.Text,
                            CustomerNo = " ",
                            CompanyName = tbxCompanyName.Text,
                            CardId = tbxCardId.Text,
                            Address = tbxAddress.Text,
                            Tel = Global.CurrentApartment.Tel
                        };
                        new CustomersLogic().AddCustomer(customer);
                        _selectedCustomer = new CustomersLogic().GetLatestCustomer();
                    }
                    if (_selectedCustomer != null)
                    {
                        Model.Room room = new Model.Room()
                        {
                            RoomNo = tbxRoomNo.Text,
                            Customer = _selectedCustomer,
                            WUnitStart = long.Parse(tbxWUnitStart.Text),
                            EUnitStart = long.Parse(tbxEUnitStart.Text),
                            MonthCost = Decimal.Parse(tbxMonthCost.Text),
                            InsureCost = Decimal.Parse(tbxInsureCost.Text),
                            StartDate = dpStartDate.SelectedDate,
                            ApartmentId = Global.CurrentApartment.ApartmentId,
                            Floor = tbxFloor.Text,
                            Picture = " ",
                            ContractMonth = tbxContractMonth.Text != "" ? long.Parse(tbxContractMonth.Text) : 0,
                            LandTaxedPerson = rbIsLandTaxedPerson.IsChecked.Value,
                            Available = rbRoomAvailable.IsChecked.Value
                        };
                        new RoomsLogic().AddRoom(room);
                        MessageBox.Show("การเพิ่มข้อมูลสำเร็จเรียบร้อย", "สำเร็จ", MessageBoxButton.OK, MessageBoxImage.Information);

                        ClearForm();

                        this.DialogResult = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "เกิดข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void InitializeForm()
        {
            rbRoomAvailable.IsChecked = true;
            rbIsNotLandTaxedPerson.IsChecked = true;
            dpStartDate.SelectedDate = DateTime.Now.Date;
            tbxCardId.Text = Global.CurrentApartment.TaxId;
            tbxCompanyName.Text = Global.CurrentApartment.CompanyName;
            tbxAddress.Text = Global.CurrentApartment.Address;
        }

        private void ClearForm()
        {
            InitializeForm();
            tbxContactName.Clear();
            tbxRoomNo.Clear();
            tbxFloor.Clear();
            tbxMonthCost.Clear();
            tbxInsureCost.Clear();
            tbxEUnitStart.Clear();
            tbxWUnitStart.Clear();
            tbxContractMonth.Clear();
        }

        private void btnSelectCustomer_Click(object sender, RoutedEventArgs e)
        {
            UI.Customer.Customer selectCustomerWindow = new UI.Customer.Customer(true);
            if (selectCustomerWindow.ShowDialog() == true)
            {
                this._selectedCustomer = selectCustomerWindow.selectedCustomer;
                tbxContactName.Text = _selectedCustomer.ContactName;
                tbxCardId.Text = _selectedCustomer.CardId;
                tbxCompanyName.Text = _selectedCustomer.CompanyName;
                tbxAddress.Text = _selectedCustomer.Address;
                this._isNewCustomer = false;
            }
        }

        private bool IsFormInputsValid()
        {
            StringBuilder validationResult = new StringBuilder();
            if (tbxContactName.Text == "") validationResult.AppendLine("- ชื่อ-นามสกุล ผู้เช่า");
            if (tbxRoomNo.Text == "") validationResult.AppendLine("- เลขที่ห้อง");
            if (tbxFloor.Text == "") validationResult.AppendLine("- ชั้น");
            if (tbxMonthCost.Text == "") validationResult.AppendLine("- ราคา/เดือน");
            if (tbxInsureCost.Text == "") validationResult.AppendLine("- ค่าประกัน");
            if (tbxWUnitStart.Text == "") validationResult.AppendLine("- มิเตอร์น้ำเริ่มต้น");
            if (tbxEUnitStart.Text == "") validationResult.AppendLine("- มิเตอร์ไฟเริ่มต้น");

            if (validationResult.ToString() != "")
            {
                validationResult.Insert(0, "กรุณาระบุข้อมูล: " + Environment.NewLine);
                MessageBox.Show(validationResult.ToString(), "เกิดข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
