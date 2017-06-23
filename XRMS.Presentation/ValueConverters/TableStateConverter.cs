using System;
using System.Globalization;
using System.Windows.Data;

using XRMS.Business.Models;

namespace XRMS.Presentation.ValueConverters
{
    public class TableStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            TableState state = (TableState)value;
            if (state == TableState.Busy)
            {
                return "/../Images/BusyTable.ico";
            }
            else if (state == TableState.Free)
            {
                return "/../Images/FreeTable.ico";
            }
            else
            {
                return "/../Images/BilledTable.ico";
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            throw new NotSupportedException("This converter is for grouping only.");
        }
    }
}
