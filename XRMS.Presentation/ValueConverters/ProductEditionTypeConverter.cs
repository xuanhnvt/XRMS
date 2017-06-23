using System;
using System.Globalization;
using System.Windows.Data;

namespace XRMS.Presentation.ValueConverters
{
    class ProductEditionTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            byte state = (byte)value;
            switch (state)
            {
                case 0:
                    return "Mới";
                case 1:
                    return "Thêm";
                case 2:
                    return "Bớt";
                case 3:
                    return "Hủy";
                default:
                    return "Unknown";
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            throw new NotSupportedException("This converter is for grouping only.");
        }
    }
}
