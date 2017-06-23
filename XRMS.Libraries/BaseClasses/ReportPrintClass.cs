using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Data;
using System.IO;

using System.Drawing.Printing;
using System.Drawing.Imaging;
using Microsoft.Reporting.WinForms;

namespace XRMS.Libraries.BaseClasses
{
    public class ReportPrintClass
    {
        private int _currentPageIndex;
        private IList<Stream> _streams;
        private double _leftMargin = 0;
        private double _rightMargin = 0;
        private double _topMargin = 0;
        private double _bottomMargin = 0;
        private double _pageWidth = 8.27;
        private double _pageHeight = 11.69;

        public ReportPrintClass(string dataSourceName, DataTable dataSourceValue, string reportPath, double pageWidth, double pageHeight , double leftMargin, double rightMargin, double topMargin, double bottomMargin)
        {
            this._pageWidth = pageWidth;
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
            _currentPageIndex = 0;
        }

        public ReportPrintClass(string dataSourceName, DataTable dataSourceValue, string reportPath, ReportParameter[] parameters, double pageWidth, double pageHeight, double leftMargin, double rightMargin, double topMargin, double bottomMargin)
        {
            this._pageWidth = pageWidth;
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
                report.SetParameters(parameters);
                Export(report);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + '\n' + e.InnerException.Message + '\n' + e.InnerException.InnerException.Message);
            }
            _currentPageIndex = 0;
        }

        // Routine to provide to the report renderer, in order to
        // save an image for each page of the report.
        private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            Stream stream = new FileStream(name + "." + fileNameExtension, FileMode.Create);
            _streams.Add(stream);
            return stream;
        }

        // Export the given report as an EMF (Enhanced Metafile) file.
        private void Export(LocalReport report)
        {
            string deviceInfo =
              "<DeviceInfo>" +
              "  <OutputFormat>EMF</OutputFormat>" +
              "  <PageWidth>" + _pageWidth + "in</PageWidth>" +
              "  <PageHeight>" + _pageHeight + "in</PageHeight>" +
              "  <MarginTop>"+_topMargin+"in</MarginTop>" +
              "  <MarginLeft>" + _leftMargin + "in</MarginLeft>" +
              "  <MarginRight>" + _rightMargin + "in</MarginRight>" +
              "  <MarginBottom>" + _bottomMargin + "in</MarginBottom>" +
              "</DeviceInfo>";
            Warning[] warnings;
            _streams = new List<Stream>();
            try
            {
                report.Render("Image", deviceInfo, CreateStream, out warnings);
            }
            catch (LocalProcessingException ex)
            {
                throw ex;
            }
            foreach (Stream stream in _streams)
                stream.Position = 0;
        }

        // Handler for PrintPageEvents
        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new Metafile(_streams[_currentPageIndex]);
            ev.Graphics.DrawImage(pageImage, ev.PageBounds);
            _currentPageIndex++;
            ev.HasMorePages = (_currentPageIndex < _streams.Count);
        }

        public void Print(string printerName, Int16 noCopy)
        {
            if (_streams == null || _streams.Count == 0)
                return;
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrinterSettings.Copies = noCopy;
            printDoc.PrinterSettings.PrinterName = printerName;
            if (!printDoc.PrinterSettings.IsValid)
            {
                string msg = String.Format(
                   "Can't find printer \"{0}\".", printerName);
                MessageBox.Show(msg, "Print Error");
                return;
            }
            printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
            try
            {
                printDoc.Print();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            if (_streams != null)
            {
                foreach (Stream stream in _streams)
                    stream.Close();
                _streams = null;
            }
        }

    }
}
