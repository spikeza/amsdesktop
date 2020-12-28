using Microsoft.Reporting.WinForms;
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

namespace AMSDesktop.UI
{
    /// <summary>
    /// Interaction logic for ReportPreviewer.xaml
    /// </summary>
    public partial class ReportPreviewer : Window
    {
        string _dataSetName;
        object _datasetValue;
        string _reportPath;

        public ReportPreviewer(string dataSetName, object datasetValue, string reportPath)
        {
            InitializeComponent();
            rvMain.Load += ReportViewer_Load;
            _dataSetName = dataSetName;
            _datasetValue = datasetValue;
            _reportPath = reportPath;
        }

        private bool _isReportViewerLoaded;

        private void ReportViewer_Load(object sender, EventArgs e)
        {
            if (!_isReportViewerLoaded)
            {
                ReportDataSource reportDataSource = new ReportDataSource(_dataSetName, _datasetValue);
                
                reportDataSource.Name = _dataSetName; //Name of the report dataset in our .RDLC file
                reportDataSource.Value = _datasetValue;
                this.rvMain.LocalReport.DataSources.Add(reportDataSource);
                this.rvMain.LocalReport.ReportPath = _reportPath;
                //this.rvMain.LocalReport.ReportPath = @".\Reports\Invoice.rdlc";

                //dataset.EndInit();

                //fill data into adventureWorksDataSet
                //AdventureWorks2008R2DataSetTableAdapters.SalesOrderDetailTableAdapter salesOrderDetailTableAdapter = new AdventureWorks2008R2DataSetTableAdapters.SalesOrderDetailTableAdapter();
                //salesOrderDetailTableAdapter.ClearBeforeFill = true;
                //salesOrderDetailTableAdapter.Fill(dataset.SalesOrderDetail);

                rvMain.RefreshReport();

                _isReportViewerLoaded = true;
            }
        }
    }
}
