using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace XRMS.Libraries.BaseClasses
{
    public class CrystalReportParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public CrystalReportParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }

    public class CrystalReportPrintClass
    {
        ReportDocument _cryRpt = new ReportDocument();
        private CrystalDecisions.Shared.PageMargins _objPageMargins;

        public CrystalReportPrintClass(string dataSourceName, DataTable dataSourceValue, string reportPath, double pageWidth, double pageHeight, double leftMargin, double rightMargin, double topMargin, double bottomMargin)
        {
            //string reportPath = Environment.CurrentDirectory + "\\ReportSrc";
            _cryRpt.Load(reportPath);
            _cryRpt.SetDataSource(dataSourceValue);
            //crystalReportViewer1.ReportSource = _cryRpt;
            //crystalReportViewer1.Refresh();


            _objPageMargins = _cryRpt.PrintOptions.PageMargins;
            _objPageMargins.bottomMargin = 0;
            _objPageMargins.leftMargin = 0;
            _objPageMargins.rightMargin = 0;
            _objPageMargins.topMargin = 0;

            //_cryRpt.SetParameterValue()
            
            _cryRpt.PrintOptions.ApplyPageMargins(_objPageMargins);
            //_cryRpt.PrintOptions.PrinterName = "Foxit Reader PDF Printer";
            //_cryRpt.PrintToPrinter(1, false, 0, 0);
            /*this._pageWidth = pageWidth;
            this._pageHeight = pageHeight;
            this._leftMargin = leftMargin;
            this._rightMargin = rightMargin;
            this._topMargin = topMargin;
            this._bottomMargin = bottomMargin;
            LocalReport report = new LocalReport();
            report.ReportPath = reportPath;
            report.DataSources.Add(new ReportDataSource(dataSourceName, dataSourceValue));
            try
            {
                Export(report);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + '\n' + e.InnerException.Message + '\n' + e.InnerException.InnerException.Message);
            }
            _currentPageIndex = 0;*/
        }

        public CrystalReportPrintClass(string dataSourceName, DataTable dataSourceValue, string reportPath, List<CrystalReportParameter> parameters, double pageWidth, double pageHeight, double leftMargin, double rightMargin, double topMargin, double bottomMargin)
        {
            //string reportPath = Environment.CurrentDirectory + "\\ReportSrc";
            _cryRpt.Load(reportPath);
            //crystalReportViewer1.ReportSource = _cryRpt;
            //crystalReportViewer1.Refresh();


            _objPageMargins = _cryRpt.PrintOptions.PageMargins;
            _objPageMargins.bottomMargin = 0;
            _objPageMargins.leftMargin = 0;
            _objPageMargins.rightMargin = 0;
            _objPageMargins.topMargin = 0;
            _cryRpt.PrintOptions.ApplyPageMargins(_objPageMargins);

            _cryRpt.SetDataSource(dataSourceValue);
            foreach (CrystalReportParameter item in parameters)
                _cryRpt.SetParameterValue(item.Name, item.Value);

            //_cryRpt.PrintOptions.PrinterName = "Foxit Reader PDF Printer";
            //_cryRpt.PrintToPrinter(1, false, 0, 0);
            /*this._pageWidth = pageWidth;
            this._pageHeight = pageHeight;
            this._leftMargin = leftMargin;
            this._rightMargin = rightMargin;
            this._topMargin = topMargin;
            this._bottomMargin = bottomMargin;
            LocalReport report = new LocalReport();
            report.ReportPath = reportPath;
            report.DataSources.Add(new ReportDataSource(dataSourceName, dataSourceValue));
            try
            {
                Export(report);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + '\n' + e.InnerException.Message + '\n' + e.InnerException.InnerException.Message);
            }
            _currentPageIndex = 0;*/
        }

        public void Print(string printerName, Int16 noCopy)
        {
            try
            {
                _cryRpt.PrintOptions.ApplyPageMargins(_objPageMargins);
                //cryRpt.PrintOptions.PrinterName = "Foxit Reader PDF Printer";
                _cryRpt.PrintOptions.PrinterName = printerName;
                _cryRpt.PrintToPrinter(noCopy, false, 0, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            /*if (_streams != null)
            {
                //foreach (Stream stream in _streams)
                    //stream.Close();
                _streams = null;
            }*/
        }

    }
}
