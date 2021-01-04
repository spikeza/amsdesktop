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
        private string _dataSetName;
        private object _datasetValue;
        private string _reportPath;
        private List<ReportParameter> _reportParameters;
        private bool _isReportViewerLoaded;

        public ReportPreviewer()
        {
            InitializeComponent();
            rvMain.Load += ReportViewer_Load;
        }

        private void ReportViewer_Load(object sender, EventArgs e)
        {
            if (!_isReportViewerLoaded)
            {
                ReportDataSource reportDataSource = new ReportDataSource(_dataSetName, _datasetValue);
                
                reportDataSource.Name = _dataSetName; //Name of the report dataset in our .RDLC file
                reportDataSource.Value = _datasetValue;
                this.rvMain.LocalReport.DataSources.Add(reportDataSource);
                this.rvMain.LocalReport.ReportPath = _reportPath;
                if (_reportParameters != null && _reportParameters.Count > 0)
                {
                    foreach (var p in _reportParameters)
                    {
                        this.rvMain.LocalReport.SetParameters(p);
                    }
                }

                rvMain.RefreshReport();

                _isReportViewerLoaded = true;
            }
        }

        public void SetDataSet(string dataSetName, object dataSetValue)
        {
            _dataSetName = dataSetName;
            _datasetValue = dataSetValue;
        }

        public void SetReportPath(string path)
        {
            _reportPath = path;
        }

        public void SetParameters(List<ReportParameter> parameters)
        {
            _reportParameters = parameters;
        }
    }
}
