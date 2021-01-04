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

namespace AMSDesktop.UI.Receipt
{
    /// <summary>
    /// Interaction logic for DeductImproveCostComfirmBox.xaml
    /// </summary>
    public partial class DeductImproveCostComfirmBox : Window
    {
        public DeductImproveCostComfirmBox()
        {
            InitializeComponent();
        }

        private void btnNoDeduct_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void btnDeduct_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
