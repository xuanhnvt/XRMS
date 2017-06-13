using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Data;

using XRMS.Business.Models;

namespace XRMS.Presentation.ValueConverters
{
    class OrderItemStateConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            OrderItemState state = (OrderItemState)value;
            switch (state)
            {
                case OrderItemState.Ordered:
                    return "/Images/OrderedProduct.ico";
                case OrderItemState.Processing:
                    return "/Images/ProcessingProduct.ico";
                case OrderItemState.Ready:
                    return "/Images/ReadyProduct.ico";
                case OrderItemState.Served:
                    return "/Images/ServedProduct.ico";
                default:
                    return "/Images/Unknown.ico";
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            throw new NotSupportedException("This converter is for grouping only.");
        }
    }
}
