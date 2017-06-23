using System;
using System.Globalization;
using System.Windows.Data;

using XRMS.Business.Models;

namespace XRMS.Presentation.ValueConverters
{
    class OrderItemStateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            OrderItemState state = (OrderItemState)value;
            switch (state)
            {
                case OrderItemState.Ordered:
                    return "Đã gọi";
                case OrderItemState.Processing:
                    return "Đang làm";
                case OrderItemState.Ready:
                    return "Sẵn sàng";
                case OrderItemState.Served:
                    return "Đã phục vụ";
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
