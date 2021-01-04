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
using AMSDesktop.BLL;
using Model = AMSDesktop.DAL.Model;

namespace AMSDesktop.UI.SystemVariable
{
    /// <summary>
    /// Interaction logic for SystemVariable.xaml
    /// </summary>
    public partial class SystemVariable : Window
    {
        private Model.SystemVariable _activeVars;
        public SystemVariable()
        {
            InitializeComponent();
            PopulateSystemVariables();
        }
        private void PopulateSystemVariables()
        {
            _activeVars = new SystemVariablesLogic().GetSystemVariable(Global.CurrentApartment.ApartmentId);
            tbxApartmentName.Text = _activeVars.BuildingName;
            tbxApartmentAddress.Text = _activeVars.OwnerAddress;
            tbxTaxId.Text = _activeVars.TaxId;
            tbxWUnitPrice.Text = _activeVars.WUnit.ToString();
            tbxEUnitPrice.Text = _activeVars.EUnit.ToString();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_activeVars != null)
                {
                    _activeVars.BuildingName = tbxApartmentName.Text;
                    _activeVars.OwnerAddress = tbxApartmentAddress.Text;
                    _activeVars.TaxId = tbxTaxId.Text.Replace("-", string.Empty);
                    _activeVars.WUnit = Single.Parse(tbxWUnitPrice.Text);
                    _activeVars.EUnit = Single.Parse(tbxEUnitPrice.Text);

                    new SystemVariablesLogic().UpdateSystemVariables(_activeVars);
                    MessageBox.Show("การแก้ไขข้อมูลสำเร็จเรียบร้อย", "สำเร็จ", MessageBoxButton.OK, MessageBoxImage.Information);

                    this.DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "เกิดข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
    }
}
