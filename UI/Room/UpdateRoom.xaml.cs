using AMSDesktop.BLL;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using Model = AMSDesktop.DAL.Model;

namespace AMSDesktop.UI.Room
{
    /// <summary>
    /// Interaction logic for UpdateRoom.xaml
    /// </summary>
    public partial class UpdateRoom : Window
    {
        private bool _isNewCustomer = false;
        private Model.Customer _selectedCustomer;
        private Model.Room _room;
        public UpdateRoom(Model.Room room)
        {
            InitializeComponent();
            _room = room;
            PopulateRoomData(_room);
        }

        private void PopulateRoomData(Model.Room room)
        {
            tbxContactName.Text = room.Customer.ContactName;
            tbxCardId.Text = room.Customer.CardId;
            tbxCompanyName.Text = room.Customer.CompanyName;
            tbxAddress.Text = room.Customer.Address;
            rbIsLandTaxedPerson.IsChecked = room.LandTaxedPerson;
            rbIsNotLandTaxedPerson.IsChecked = !room.LandTaxedPerson;
            rbRoomAvailable.IsChecked = room.Available;
            rbRoomUnavailable.IsChecked = !room.Available;
            tbxRoomNo.Text = room.RoomNo;
            tbxFloor.Text = room.Floor;
            tbxMonthCost.Text = room.MonthCost.ToString();
            tbxInsureCost.Text = room.InsureCost.ToString();
            tbxWUnitStart.Text = room.WUnitStart.ToString();
            tbxEUnitStart.Text = room.EUnitStart.ToString();
            tbxContractMonth.Text = room.ContractMonth.ToString();
            dpStartDate.SelectedDate = room.StartDate.Value;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsFormInputsValid())
                {
                    if (tbxContactName.Text != _room.Customer.ContactName)
                    {
                        if (_selectedCustomer == null)
                        {
                            if (new CustomersLogic().SearchCustomers(tbxContactName.Text).Count > 0)
                            {
                                UI.Customer.Customer selectCustomerWindow = new UI.Customer.Customer(true, tbxContactName.Text);
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
                            else
                            {
                                this._isNewCustomer = true;
                            }
                        }
                    }
                    else
                    {
                        _selectedCustomer = _room.Customer;
                    }
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
                        _room.RoomNo = tbxRoomNo.Text;
                        _room.Customer = _selectedCustomer;
                        _room.WUnitStart = long.Parse(tbxWUnitStart.Text);
                        _room.EUnitStart = long.Parse(tbxEUnitStart.Text);
                        _room.MonthCost = Decimal.Parse(tbxMonthCost.Text);
                        _room.InsureCost = Decimal.Parse(tbxInsureCost.Text);
                        _room.StartDate = dpStartDate.SelectedDate;
                        _room.ApartmentId = Global.CurrentApartment.ApartmentId;
                        _room.Floor = tbxFloor.Text;
                        _room.Picture = " ";
                        _room.ContractMonth = tbxContractMonth.Text != "" ? long.Parse(tbxContractMonth.Text) : 0;
                        _room.LandTaxedPerson = rbIsLandTaxedPerson.IsChecked.Value;
                        _room.Available = rbRoomAvailable.IsChecked.Value;
                        new RoomsLogic().UpdateRoom(_room);

                        MessageBox.Show("การแก้ไขข้อมูลสำเร็จเรียบร้อย", "สำเร็จ", MessageBoxButton.OK, MessageBoxImage.Information);

                        this.DialogResult = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "เกิดข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
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
