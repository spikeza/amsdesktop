using System;
using System.Collections.Generic;
using System.Globalization;
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
using Model = AMSDesktop.DAL.Model;
using GreatFriends.ThaiBahtText;
using AMSDesktop.BLL;

namespace AMSDesktop.UI.Receipt
{
    /// <summary>
    /// Interaction logic for UpdateReceipt.xaml
    /// </summary>
    public partial class UpdateReceipt : Window
    {
        Model.Receipt _receipt;
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
        public UpdateReceipt(Model.Receipt receipt)
        {
            InitializeComponent();
            _receipt = receipt;
            PopulateRoomNoDropDown();
            PopulateReceiptData(_receipt);
            EnablePrinting(false);
        }

        private void PopulateRoomNoDropDown()
        {
            List<Model.RoomDropDownView> rooms = new RoomsLogic().GetRoomsForDropDownList(Global.CurrentApartment.ApartmentId);
            cbbRoomNo.ItemsSource = rooms;
            cbbRoomNo.DisplayMemberPath = "RoomNo";
            cbbRoomNo.SelectedValuePath = "RoomId";
        }

        private void PopulateReceiptData(Model.Receipt receipt)
        {
            tbkApartmentName.Text = Global.CurrentApartment.ApartmentName;
            tbkApartmentAddress.Text = Global.CurrentApartment.Address;
            tbxReceiptNo.Text = receipt.ReceiptNo;
            cbbRoomNo.SelectedValue = receipt.Invoice.Room.RoomId;
            tbxMonth.Text = receipt.Invoice.MonthNo.ToString();
            tbxWaterStart.Text = receipt.Invoice.WMeterStart.ToString();
            tbxElectricStart.Text = receipt.Invoice.EMeterStart.ToString();
            tbxWaterEnd.Text = (receipt.Invoice.WMeterStart + receipt.Invoice.WUsedUnit).ToString();
            tbxElectricEnd.Text = (receipt.Invoice.EMeterStart + receipt.Invoice.EUsedUnit).ToString();
            tbxWaterUnitPrices.Text = receipt.Invoice.WUnit.ToString("N2", thCulture);
            tbxElectricUnitPrices.Text = receipt.Invoice.EUnit.ToString("N2", thCulture);
            tbxWaterUnits.Text = receipt.Invoice.WUsedUnit.ToString();
            tbxElectricUnits.Text = receipt.Invoice.EUsedUnit.ToString();
            tbxTelephoneAmount.Text = receipt.Invoice.TelCost.ToString("N2", thCulture);
            tbxMonthCost.Text = receipt.Invoice.Room.MonthCost.ToString("N2", thCulture);
            tbxImproveCost.Text = receipt.Invoice.ImproveCost.ToString("N2", thCulture);
            tbxImproveText.Text = receipt.Invoice.ImproveText;
            tbxComment.Text = receipt.Comment;

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

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (IsFormInputValid())
            {
                try
                {
                    _receipt.Comment = tbxComment.Text == "" ? " " : tbxComment.Text;
                    new ReceiptsLogic().UpdateReceipt(_receipt);
                    MessageBox.Show("การแก้ไขข้อมูลสำเร็จเรียบร้อย", "สำเร็จ", MessageBoxButton.OK, MessageBoxImage.Information);

                    EnablePrinting(true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "เกิดข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (_receipt != null)
            {
                ReportPreviewer rp = new ReportPreviewer();
                rp.SetDataSet("ReceiptDataSet", new ReceiptsLogic().GetReceiptForPrinting(_receipt));
                rp.SetReportPath(@".\Reports\Receipt.rdlc"); 
                rp.ShowDialog();
            }
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
                tbxComment.IsReadOnly = true;
                btnUpdate.IsEnabled = false;
                btnPrint.IsEnabled = true;
            }
            else
            {
                tbxComment.IsReadOnly = false;
                btnUpdate.IsEnabled = true;
                btnPrint.IsEnabled = false;
            }
        }
    }
}
