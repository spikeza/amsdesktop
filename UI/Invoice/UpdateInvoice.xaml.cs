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
    /// Interaction logic for UpdateInvoice.xaml
    /// </summary>
    public partial class UpdateInvoice : Window
    {
        Model.Invoice _invoice;
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
        bool _isInputFormatValid;
        public UpdateInvoice(Model.Invoice invoice)
        {
            InitializeComponent();
            _invoice = invoice;
            PopulateRoomNoDropDown();
            PopulateInvoiceData(_invoice);
        }

        private void PopulateRoomNoDropDown()
        {
            List<Model.RoomDropDownView> rooms = new RoomsLogic().GetRoomsForDropDownList(Global.CurrentApartment.ApartmentId);
            cbbRoomNo.ItemsSource = rooms;
            cbbRoomNo.DisplayMemberPath = "RoomNo";
            cbbRoomNo.SelectedValuePath = "RoomId";
        }

        private void PopulateInvoiceData(Model.Invoice invoice)
        {
            tbkApartmentName.Text = Global.CurrentApartment.ApartmentName;
            tbkApartmentAddress.Text = Global.CurrentApartment.Address;
            tbxInvoiceNo.Text = invoice.InvoiceNo;
            cbbRoomNo.SelectedValue = invoice.Room.RoomId;
            tbxMonth.Text = invoice.MonthNo.ToString();
            tbxWaterStart.Text = invoice.WMeterStart.ToString();
            tbxElectricStart.Text = invoice.EMeterStart.ToString();
            tbxWaterEnd.Text = (invoice.WMeterStart + invoice.WUsedUnit).ToString();
            tbxElectricEnd.Text = (invoice.EMeterStart + invoice.EUsedUnit).ToString();
            tbxWaterUnitPrices.Text = invoice.WUnit.ToString("N2", thCulture);
            tbxElectricUnitPrices.Text = invoice.EUnit.ToString("N2", thCulture);
            tbxWaterUnits.Text = invoice.WUsedUnit.ToString();
            tbxElectricUnits.Text = invoice.EUsedUnit.ToString();
            tbxTelephoneAmount.Text = invoice.TelCost.ToString("N2", thCulture);
            tbxMonthCost.Text = invoice.Room.MonthCost.ToString("N2", thCulture);
            tbxImproveCost.Text = invoice.ImproveCost.ToString("N2", thCulture);
            tbxImproveText.Text = invoice.ImproveText;
            tbxComment.Text = invoice.Comment;

            CalculateAllAmounts();
        }
        private void CalculateAllAmounts()
        {
            if (!Decimal.TryParse(tbxImproveCost.Text, out _improveCost) || !Decimal.TryParse(tbxTelephoneAmount.Text, out _telephoneAmount))
            {
                _isInputFormatValid = false;
                SetInputTextBoxesRed(true);
            }
            else
            {
                _isInputFormatValid = true;
                SetInputTextBoxesRed(false);
            }

            if (_isInputFormatValid)
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
        }


        private void tbxWaterEnd_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateAllAmounts();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void FloatingValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            if (((TextBox)sender).Text.Contains("."))
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

        private void tbxElectricEnd_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateAllAmounts();
        }
        private void SetInputTextBoxesRed(bool isRed)
        {
            if (isRed)
            {
                tbxTelephoneAmount.BorderBrush = System.Windows.Media.Brushes.Red;
                tbxImproveCost.BorderBrush = System.Windows.Media.Brushes.Red;
            }
            else
            {
                tbxTelephoneAmount.ClearValue(Border.BorderBrushProperty);
                tbxImproveCost.ClearValue(Border.BorderBrushProperty);
            }
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

        private bool IsFormInputValid()
        {
            StringBuilder errorList = new StringBuilder();

            if (tbxInvoiceNo.Text == "")
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

            if (!_isInputFormatValid)
            {
                MessageBox.Show("กรุณาตรวจสอบข้อมูลจำนวนเงินให้ถูกต้อง", "รูปแบบข้อมูลไม่ถูกต้อง", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;

        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (IsFormInputValid())
            {
                try
                {
                    _invoice.WUsedUnit = _waterUnit;
                    _invoice.EUsedUnit = _electricUnit;
                    _invoice.TelCost = _telephoneAmount;
                    _invoice.ImproveCost = _improveCost;
                    _invoice.ImproveText = tbxImproveText.Text;
                    _invoice.Comment = tbxComment.Text == "" ? " " : tbxComment.Text;
                    _invoice.TotalText = ThaiBahtTextUtil.ThaiBahtText(_totalAmount);
                    _invoice.GrandTotal = Decimal.ToSingle(_grandTotalAmount);
                    _invoice.GrandTotalText = ThaiBahtTextUtil.ThaiBahtText(_grandTotalAmount);
                    new InvoicesLogic().UpdateInvoice(_invoice);
                    MessageBox.Show("การแก้ไขข้อมูลสำเร็จเรียบร้อย", "สำเร็จ", MessageBoxButton.OK, MessageBoxImage.Information);

                    _invoice.Room.WUnitStart = _waterStart + _waterUnit;
                    _invoice.Room.EUnitStart = _electricStart + _electricUnit;
                    new RoomsLogic().UpdateRoomMeterStart(_invoice.Room);

                    EnablePrinting(true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "เกิดข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (_invoice != null)
            {
                ReportPreviewer rp = new ReportPreviewer();
                rp.SetDataSet("InvoiceDataSet", new InvoicesLogic().GetInvoiceForPrinting(_invoice));
                rp.SetReportPath(@".\Reports\Invoice.rdlc");
                rp.ShowDialog();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void EnablePrinting(bool isEnable)
        {
            if (isEnable)
            {
                tbxInvoiceNo.IsReadOnly = true;
                cbbRoomNo.IsEnabled = false;
                tbxMonth.IsReadOnly = true;
                tbxWaterEnd.IsReadOnly = true;
                tbxElectricEnd.IsReadOnly = true;
                tbxTelephoneAmount.IsReadOnly = true;
                tbxImproveCost.IsReadOnly = true;
                tbxImproveText.IsReadOnly = true;
                tbxComment.IsReadOnly = true;
                btnUpdate.IsEnabled = false;
                btnPrint.IsEnabled = true;
            }
            else
            {
                tbxInvoiceNo.IsReadOnly = false;
                cbbRoomNo.IsEnabled = true;
                tbxMonth.IsReadOnly = false;
                tbxWaterEnd.IsReadOnly = false;
                tbxElectricEnd.IsReadOnly = false;
                tbxTelephoneAmount.IsReadOnly = false;
                tbxImproveCost.IsReadOnly = false;
                tbxImproveText.IsReadOnly = false;
                tbxComment.IsReadOnly = false;
                btnUpdate.IsEnabled = true;
                btnPrint.IsEnabled = false;
            }
        }
    }
}
