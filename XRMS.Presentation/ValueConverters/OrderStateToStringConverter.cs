using System;
using System.Globalization;
using System.Windows.Data;

using XRMS.Business.Models;

namespace XRMS.Presentation.ValueConverters
{
    public class OrderStateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            OrderState state = (OrderState)value;
            switch (state)
            {
                case OrderState.Ordered:
                    return "Đã gọi món";
                case OrderState.Serving:
                    return "Đang phục vụ";
                case OrderState.Served:
                    return "Đã phục vụ";
                case OrderState.Printed:
                    return "Đã in hóa đơn";
                case OrderState.Billed:
                    return "Đã thanh toán";
                case OrderState.Finished:
                    return "Hoàn tất";
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
