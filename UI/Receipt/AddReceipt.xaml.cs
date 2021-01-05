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
using System.Configuration;

namespace AMSDesktop.UI.Receipt
{
    /// <summary>
    /// Interaction logic for AddReceipt.xaml
    /// </summary>
    public partial class AddReceipt : Window
    {
        private CultureInfo thCulture = new CultureInfo("th-TH");
        private Model.Room _selectedRoom;
        private int _selectedMonth;
        private Model.Invoice _relatedInvoice;
        private Model.Receipt _activeReceipt;
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
        public AddReceipt()
        {
            InitializeComponent();
            PopulateFieldsOnLoad();
            EnablePrinting(false);
        }

        private void PopulateFieldsOnLoad()
        {
            tbkApartmentName.Text = Global.CurrentApartment.ApartmentName;
            tbkApartmentAddress.Text = Global.CurrentApartment.Address;
            tbxReceiptNo.Text = new ReceiptsLogic().GetNewReceiptNumber(Global.CurrentApartment.ApartmentId);
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
            PopulateReceiptData();
        }

        private void PopulateFieldsOnRoomSelect()
        {
            Model.Invoice searchInvoice;
            if (DateTime.Now.Month == 1 && _selectedMonth == 12)
                searchInvoice = new InvoicesLogic().GetInvoiceForReceipt(_selectedRoom.RoomId, _selectedMonth, DateTime.Now.Year-1);
            else
                searchInvoice = new InvoicesLogic().GetInvoiceForReceipt(_selectedRoom.RoomId, _selectedMonth, DateTime.Now.Year);
            if (searchInvoice != null)
            {
                _relatedInvoice = searchInvoice;
                tbkApartmentName.Text = Global.CurrentApartment.ApartmentName;
                tbkApartmentAddress.Text = Global.CurrentApartment.Address;
                tbxReceiptNo.Text = _relatedInvoice.InvoiceNo;
                cbbRoomNo.SelectedValue = _relatedInvoice.Room.RoomId;
                tbxMonth.Text = _relatedInvoice.MonthNo.ToString();
                tbxWaterStart.Text = _relatedInvoice.WMeterStart.ToString();
                tbxElectricStart.Text = _relatedInvoice.EMeterStart.ToString();
                tbxWaterEnd.Text = (_relatedInvoice.WMeterStart + _relatedInvoice.WUsedUnit).ToString();
                tbxElectricEnd.Text = (_relatedInvoice.EMeterStart + _relatedInvoice.EUsedUnit).ToString();
                tbxWaterUnitPrices.Text = _relatedInvoice.WUnit.ToString("N2", thCulture);
                tbxElectricUnitPrices.Text = _relatedInvoice.EUnit.ToString("N2", thCulture);
                tbxWaterUnits.Text = _relatedInvoice.WUsedUnit.ToString();
                tbxElectricUnits.Text = _relatedInvoice.EUsedUnit.ToString();
                tbxTelephoneAmount.Text = _relatedInvoice.TelCost.ToString("N2", thCulture);
                tbxMonthCost.Text = _relatedInvoice.Room.MonthCost.ToString("N2", thCulture);
                tbxImproveCost.Text = _relatedInvoice.ImproveCost.ToString("N2", thCulture);
                tbxImproveText.Text = _relatedInvoice.ImproveText;
                tbxComment.Text = _relatedInvoice.Comment;

                CalculateAllAmounts();
            }
            else
            {
                ClearForm();
                MessageBox.Show("ไม่พบใบแจ้งหนี้ของเดือนที่กำหนด", "เกิดข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
            }                
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

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (IsFormInputValid())
            {
                try
                {
                    string utilitiesVATInComment = " ";
                    string vat = ConfigurationManager.AppSettings["VAT"].ToString();
                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["ShowUtilitiesVATInComment"].ToString()))
                    {
                        Decimal utilityAmount = (_relatedInvoice.WUsedUnit * _relatedInvoice.WUnit) + (_relatedInvoice.EUsedUnit * _relatedInvoice.EUnit);
                        Decimal divisor = 1 + (Decimal.Parse(vat) / 100);
                        Decimal utilityAmountBeforeTax = utilityAmount / divisor;
                        Decimal vatAmount = utilityAmount - utilityAmountBeforeTax;
                        utilitiesVATInComment = string.Format("มูลค่าเพิ่ม {0}% ค่าน้ำประปา, ค่าไฟฟ้า {1} บาท", vat, vatAmount.ToString("N2"));
                    }
                    Model.Receipt receipt = new Model.Receipt()
                    {
                        ApartmentId = Global.CurrentApartment.ApartmentId,
                        ReceiptNo = tbxReceiptNo.Text,
                        Invoice = _relatedInvoice,
                        InterestUnit = Convert.ToDecimal(0),
                        AmountDay = 0,
                        RcpDate = DateTime.Now.Date,
                        Comment = tbxComment.Text == "" ? utilitiesVATInComment : utilitiesVATInComment + System.Environment.NewLine + tbxComment.Text,
                        TotalText = ThaiBahtTextUtil.ThaiBahtText(_totalAmount),
                        GrandTotal = Decimal.ToSingle(_grandTotalAmount),
                        GrandTotalText = ThaiBahtTextUtil.ThaiBahtText(_grandTotalAmount)
                    };

                    new ReceiptsLogic().AddReceipt(receipt);
                    _activeReceipt = receipt;
                    _activeReceipt.Invoice.Paid = true;
                    new InvoicesLogic().SetInvoicePaidStatus(_relatedInvoice);

                    MessageBox.Show("การเพิ่มข้อมูลสำเร็จเรียบร้อย", "สำเร็จ", MessageBoxButton.OK, MessageBoxImage.Information);

                    EnablePrinting(true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "เกิดข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (_activeReceipt != null)
            {
                string reportPath = @".\Reports\Receipt.rdlc";
                List<Model.ReceiptForPrinting> receipts = new ReceiptsLogic().GetReceiptForPrinting(_activeReceipt);

                DeductImproveCostComfirmBox confirmBox = new DeductImproveCostComfirmBox();
                confirmBox.WindowStartupLocation = WindowStartupLocation.Manual;
                confirmBox.Top = Mouse.GetPosition(null).Y - 200;
                confirmBox.Left = Mouse.GetPosition(null).X;
                if (confirmBox.ShowDialog() == false)
                {
                    reportPath = @".\Reports\ReceiptDeductImproveCost.rdlc";
                    foreach (var r in receipts)
                    {
                        r.GrandTotalText = ThaiBahtTextUtil.ThaiBahtText(Convert.ToDecimal(r.GrandTotal) - r.ImproveCost);                  
                    }
                }

                ReportPreviewer rp = new ReportPreviewer();
                rp.SetDataSet("ReceiptDataSet", receipts);
                rp.SetReportPath(reportPath);
                rp.ShowDialog();
            }
        }

        private void SetInputTextBoxesRed(bool isRed)
        {
            if (isRed)
            {
                tbxMonth.BorderBrush = System.Windows.Media.Brushes.Red;
            }
            else
            {
                tbxMonth.ClearValue(Border.BorderBrushProperty);
            }
        }

        private bool IsMonthInputValid()
        {
            _selectedMonth = 0;
            if (int.TryParse(tbxMonth.Text, out int month))
            {
                if (month >= 1 && month <= 12)
                {
                    _selectedMonth = month;
                    SetInputTextBoxesRed(false);
                    return true;
                }
                SetInputTextBoxesRed(true);
            }
            else
            {
                SetInputTextBoxesRed(true);
            }

            MessageBox.Show("กรุณาระบุค่าเดือนระหว่าง 1 - 12", "รูปแบบข้อมูลไม่ถูกต้อง", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        private void tbxMonth_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PopulateReceiptData();
            }
        }

        private void ClearForm()
        {
            _selectedRoom = null;
            _selectedMonth = 0;
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

        private bool IsFormInputValid()
        {
            StringBuilder errorList = new StringBuilder();

            if (tbxReceiptNo.Text == "")
                errorList.AppendLine("เลขที่ใบแจ้งหนี้/Invoice No");
            if (cbbRoomNo.SelectedItem == null)
                errorList.AppendLine("เลขที่ห้องพัก/Room No");
            if (tbxMonth.Text == "")
                errorList.AppendLine("ประจำเดือน/Month");
            if (tbxWaterStart.Text == "" || tbxWaterEnd.Text == "")
                errorList.AppendLine("ค่าน้ำประปา (Water Supply)");
            if (tbxElectricStart.Text == "" || tbxElectricEnd.Text == "")
                errorList.AppendLine("ค่าไฟฟ้า (Electric Cost)");
            if (tbxTelephoneAmount.Text == "")
                errorList.AppendLine("ค่าโทรศัพท์ (Telephone Fee");
            if (tbxMonthCost.Text == "")
                errorList.AppendLine("ค่าเช่าห้อง");
            if (tbxImproveCost.Text == "")
                errorList.AppendLine("ค่าใช้จ่ายอื่น ๆ");

            if (errorList.ToString() != "")
            {
                MessageBox.Show("กรุณาระบุข้อมูลต่อไปนี้: \r\n\r\n" + errorList.ToString(), "ข้อมูลที่จำเป็นไม่ครบถ้วน", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void EnablePrinting(bool isEnable)
        {
            if (isEnable)
            {
                cbbRoomNo.IsEnabled = false;
                tbxMonth.IsReadOnly = true;
                tbxComment.IsReadOnly = true;
                btnAdd.IsEnabled = false;
                btnClear.IsEnabled = false;
                btnPrint.IsEnabled = true;
            }
            else
            {
                cbbRoomNo.IsEnabled = true;
                tbxMonth.IsReadOnly = false;
                tbxComment.IsReadOnly = false;
                btnAdd.IsEnabled = true;
                btnClear.IsEnabled = true;
                btnPrint.IsEnabled = false;
            }
        }

        private void PopulateReceiptData()
        {
            if (IsMonthInputValid())
            {
                if (cbbRoomNo.SelectedValue != null)
                {
                    _selectedRoom = new RoomsLogic().GetRoom((long)cbbRoomNo.SelectedValue);
                    int receiptYear = (DateTime.Now.Month == 1 && _selectedMonth == 12) ? DateTime.Now.Year - 1 : DateTime.Now.Year;
                    if (!new ReceiptsLogic().IsThisMonthReceiptExists(_selectedRoom.RoomId, long.Parse(tbxMonth.Text), receiptYear))
                    {
                        PopulateFieldsOnRoomSelect();
                    }
                    else
                    {
                        ClearForm();
                        MessageBox.Show("มีข้อมูลใบเสร็จรับเงินของเดือนนี้ในระบบแล้ว", "เกิดข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
