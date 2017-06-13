using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Data;

using XRMS.Business.Models;

namespace XRMS.Presentation.ValueConverters
{
    class OrderStateConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            OrderState state = (OrderState)value;
            switch (state)
            {
                case OrderState.Ordered:
                    return "/Images/Ordered.ico";
                case OrderState.Serving:
                    return "/Images/ServingOrder.ico";
                case OrderState.Served:
                    return "/Images/ServedOrder.ico";
                case OrderState.Printed:
                    return "/Images/PrintedOrder.ico";
                case OrderState.Billed:
                    return "/Images/BilledOrder.ico";
                case OrderState.Finished:
                    return "/Images/FinishedOrder.ico";
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
