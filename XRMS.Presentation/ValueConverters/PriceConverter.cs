using System;
using System.Globalization;
using System.Windows.Data;

namespace XRMS.Presentation.ValueConverters
{
    class PriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
       CultureInfo culture)
        {
            string temp = value.ToString();
            if (temp.EndsWith(".00"))
            {
                temp = temp.Split('.')[0];
            }

            /*return temp + " $";*/
            return temp;
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            //throw new NotSupportedException("This converter is for grouping only.");
            //return value.ToString();
            return System.Convert.ToDecimal(value);
        }
    }
}
