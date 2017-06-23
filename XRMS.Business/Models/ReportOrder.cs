using System;
using System.Collections.Generic;

using Csla;

using XRMS.Libraries.CslaBase;
using XRMS.Libraries.Constants;

namespace XRMS.Business.Models
{
    [Serializable]
    public class ReportOrder : CslaBusinessBase<ReportOrder>
    {
        #region Private Data Members

        List<ReportOrderItem> _orderItemList;

        #endregion // Private Data Members

        #region Constructors

        public ReportOrder() : base()
        {
            // set default time
            this.OrderDatetime = Convert.ToDateTime(DateTimeFormats.YMDHMS_NULLDATE);
            this.EditDatetime = Convert.ToDateTime(DateTimeFormats.YMDHMS_NULLDATE);
            this.BillDatetime = Convert.ToDateTime(DateTimeFormats.YMDHMS_NULLDATE);
            this.CheckoutDatetime = Convert.ToDateTime(DateTimeFormats.YMDHMS_NULLDATE);
        }

        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the report counter.
        /// </summary>
        /// <value>
        /// The report counter.
        /// </value>
        public long ReportCounter
        {
            get { return GetProperty(ReportCounterProperty); }
            set { SetProperty(ReportCounterProperty, value); }
        }
        public static readonly PropertyInfo<long> ReportCounterProperty = RegisterProperty<long>(p => p.ReportCounter);

        /// <summary>
        /// Gets or sets the table code that matches with this order.
        /// </summary>
        /// <value>
        /// The table code.
        /// </value>
        public string TableCode
        {
            get { return GetProperty(TableCodeProperty); }
            set { SetProperty(TableCodeProperty, value); }
        }
        public static readonly PropertyInfo<string> TableCodeProperty = RegisterProperty<string>(p => p.TableCode);

        /// <summary>
        /// Gets or sets the table name that matches with this order.
        /// </summary>
        /// <value>
        /// The table name.
        /// </value>
        public string TableName
        {
            get { return GetProperty(TableNameProperty); }
            set { SetProperty(TableNameProperty, value); }
        }
        public static readonly PropertyInfo<string> TableNameProperty = RegisterProperty<string>(p => p.TableName);

        /// <summary>
        /// Gets or sets the user name that creates this order.
        /// </summary>
        /// <value>
        /// The user name.
        /// </value>
        public string CreatorName
        {
            get { return GetProperty(CreatorNameProperty); }
            set { SetProperty(CreatorNameProperty, value); }
        }
        public static readonly PropertyInfo<string> CreatorNameProperty = RegisterProperty<string>(p => p.CreatorName);

        /// <summary>
        /// Gets or sets the order code.
        /// </summary>
        /// <value>
        /// The order code.
        /// </value>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        public static readonly PropertyInfo<string> CodeProperty = RegisterProperty<string>(p => p.Code);

        /// <summary>
        /// Gets or sets the order datetime.
        /// </summary>
        /// <value>
        /// The the order datetime.
        /// </value>
        public DateTime OrderDatetime
        {
            get { return GetProperty(OrderDatetimeProperty); }
            set { SetProperty(OrderDatetimeProperty, value); NotifyPropertyChanged(() => OrderDatetimeString); }
        }
        public static readonly PropertyInfo<DateTime> OrderDatetimeProperty = RegisterProperty<DateTime>(p => p.OrderDatetime);

        /// <summary>
        /// Gets or sets the order datetime.
        /// </summary>
        /// <value>
        /// The order datetime.
        /// </value>
        public string OrderDatetimeString
        {
            get
            {
                if (OrderDatetime != null)
                {
                    if (OrderDatetime != Convert.ToDateTime(DateTimeFormats.YMDHMS_NULLDATE))
                    {
                        return ((DateTime)OrderDatetime).ToString("dd-MM HH:mm");
                    }
                    else
                        return String.Empty;
                }
                return String.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the last editting datetime.
        /// </summary>
        /// <value>
        /// The last editting datetime.
        /// </value>
        public DateTime EditDatetime
        {
            get { return GetProperty(EditDatetimeProperty); }
            set { SetProperty(EditDatetimeProperty, value); NotifyPropertyChanged(() => EditDatetimeString); }
        }
        public static readonly PropertyInfo<DateTime> EditDatetimeProperty = RegisterProperty<DateTime>(p => p.EditDatetime);

        /// <summary>
        /// Gets or sets the last editting datetime.
        /// </summary>
        /// <value>
        /// The last editting datetime.
        /// </value>
        public string EditDatetimeString
        {
            get
            {
                if (EditDatetime != null)
                {
                    if (EditDatetime != Convert.ToDateTime(DateTimeFormats.YMDHMS_NULLDATE))
                    {
                        return ((DateTime)EditDatetime).ToString("dd-MM HH:mm");
                    }
                    else
                        return String.Empty;
                }
                return String.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the order billing datetime.
        /// </summary>
        /// <value>
        /// The billing datetime.
        /// </value>
        public DateTime BillDatetime
        {
            get { return GetProperty(BillDatetimeProperty); }
            set { SetProperty(BillDatetimeProperty, value); NotifyPropertyChanged(() => BillDatetimeString); }
        }
        public static readonly PropertyInfo<DateTime> BillDatetimeProperty = RegisterProperty<DateTime>(p => p.BillDatetime);

        /// <summary>
        /// Gets or sets the order billing datetime.
        /// </summary>
        /// <value>
        /// The order billing datetime.
        /// </value>
        public string BillDatetimeString
        {
            get
            {
                if (BillDatetime != null)
                {
                    if (BillDatetime != Convert.ToDateTime(DateTimeFormats.YMDHMS_NULLDATE))
                    {
                        return ((DateTime)BillDatetime).ToString("dd-MM HH:mm");
                    }
                    else
                        return String.Empty;
                }
                return String.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the check out datetime.
        /// </summary>
        /// <value>
        /// The check out datetime.
        /// </value>
        public DateTime CheckoutDatetime
        {
            get { return GetProperty(CheckoutDatetimeProperty); }
            set { SetProperty(CheckoutDatetimeProperty, value); NotifyPropertyChanged(() => CheckoutDatetimeString); }
        }
        public static readonly PropertyInfo<DateTime> CheckoutDatetimeProperty = RegisterProperty<DateTime>(p => p.CheckoutDatetime);

        /// <summary>
        /// Gets or sets the check out datetime.
        /// </summary>
        /// <value>
        /// The check out datetime.
        /// </value>
        public string CheckoutDatetimeString
        {
            get
            {
                if (CheckoutDatetime != null)
                {
                    if (CheckoutDatetime != Convert.ToDateTime(DateTimeFormats.YMDHMS_NULLDATE))
                    {
                        return ((DateTime)CheckoutDatetime).ToString("dd-MM HH:mm");
                    }
                    else
                        return String.Empty;
                }
                return String.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the order state.
        /// </summary>
        /// <value>
        /// The order state.
        /// </value>
        public OrderState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        public static readonly PropertyInfo<OrderState> StateProperty = RegisterProperty<OrderState>(p => p.State);

        /// <summary>
        /// Gets or sets the sub total price.
        /// </summary>
        /// <value>
        /// The sub total price.
        /// </value>
        public decimal SubTotalPrice
        {
            get { return GetProperty(SubTotalPriceProperty); }
            set { SetProperty(SubTotalPriceProperty, value); }
        }
        public static readonly PropertyInfo<decimal> SubTotalPriceProperty = RegisterProperty<decimal>(p => p.SubTotalPrice);

        /// <summary>
        /// Gets or sets the sub total price.
        /// </summary>
        /// <value>
        /// The total price.
        /// </value>
        public decimal TotalPrice
        {
            get { return GetProperty(TotalPriceProperty); }
            set { SetProperty(TotalPriceProperty, value); }
        }
        public static readonly PropertyInfo<decimal> TotalPriceProperty = RegisterProperty<decimal>(p => p.TotalPrice);

        /// <summary>
        /// Gets or sets the service charge.
        /// </summary>
        /// <value>
        /// The service charge.
        /// </value>
        public decimal ServiceCharge
        {
            get { return GetProperty(ServiceChargeProperty); }
            set { SetProperty(ServiceChargeProperty, value); }
        }
        public static readonly PropertyInfo<decimal> ServiceChargeProperty = RegisterProperty<decimal>(p => p.ServiceCharge);

        /// <summary>
        /// Gets or sets the logic of enabling calculation VAT or not .
        /// </summary>
        /// <value>
        /// The vat enabling logic.
        /// </value>
        public bool VatEnable
        {
            get { return GetProperty(VatEnableProperty); }
            set { SetProperty(VatEnableProperty, value); }
        }
        public static readonly PropertyInfo<bool> VatEnableProperty = RegisterProperty<bool>(p => p.VatEnable);

        /// <summary>
        /// Gets or sets the VAT price.
        /// </summary>
        /// <value>
        /// The VAT price.
        /// </value>
        public decimal VatPrice
        {
            get { return GetProperty(VatPriceProperty); }
            set { SetProperty(VatPriceProperty, value); }
        }
        public static readonly PropertyInfo<decimal> VatPriceProperty = RegisterProperty<decimal>(p => p.VatPrice);

        /// <summary>
        /// Gets or sets the percent number of discount if there.
        /// </summary>
        /// <value>
        /// The discount percent number (0 - 100).
        /// </value>
        public byte DiscountPercent
        {
            get { return GetProperty(DiscountPercentProperty); }
            set { SetProperty(DiscountPercentProperty, value); }
        }
        public static readonly PropertyInfo<byte> DiscountPercentProperty = RegisterProperty<byte>(p => p.DiscountPercent);

        /// <summary>
        /// Gets or sets the discount price.
        /// </summary>
        /// <value>
        /// The discount price.
        /// </value>
        public decimal DiscountPrice
        {
            get { return GetProperty(DiscountPriceProperty); }
            set { SetProperty(DiscountPriceProperty, value); }
        }
        public static readonly PropertyInfo<decimal> DiscountPriceProperty = RegisterProperty<decimal>(p => p.DiscountPrice);

        /// <summary>
        /// Gets or sets the special discount price.
        /// </summary>
        /// <value>
        /// The special discount price (amount of money).
        /// </value>
        public decimal SpecialDiscount
        {
            get { return GetProperty(SpecialDiscountProperty); }
            set { SetProperty(SpecialDiscountProperty, value); }
        }
        public static readonly PropertyInfo<decimal> SpecialDiscountProperty = RegisterProperty<decimal>(p => p.SpecialDiscount);

        /// <summary>
        /// Gets or sets the cash (amount of money from customer).
        /// </summary>
        /// <value>
        /// The cash.
        /// </value>
        public decimal Cash
        {
            get { return GetProperty(CashProperty); }
            set { SetProperty(CashProperty, value); }
        }
        public static readonly PropertyInfo<decimal> CashProperty = RegisterProperty<decimal>(p => p.Cash);

        /// <summary>
        /// Gets or sets the change (amount of money return back customer).
        /// </summary>
        /// <value>
        /// The change.
        /// </value>
        public decimal Change
        {
            get { return GetProperty(ChangeProperty); }
            set { SetProperty(ChangeProperty, value); }
        }
        public static readonly PropertyInfo<decimal> ChangeProperty = RegisterProperty<decimal>(p => p.Change);

        /// <summary>
        /// Gets or sets the print count (the order is printed or not).
        /// </summary>
        /// <value>
        /// The print count.
        /// </value>
        public byte PrintCount
        {
            get { return GetProperty(PrintCountProperty); }
            set { SetProperty(PrintCountProperty, value); }
        }
        public static readonly PropertyInfo<byte> PrintCountProperty = RegisterProperty<byte>(p => p.PrintCount);

        /// <summary>
        /// Gets or sets the flag IsCancelled (check if order is cancelled or not)
        /// </summary>
        /// <value>
        /// The flag IsCancelled.
        /// </value>
        public bool IsCancelled
        {
            get { return GetProperty(IsCancelledProperty); }
            set { SetProperty(IsCancelledProperty, value); }
        }
        public static readonly PropertyInfo<bool> IsCancelledProperty = RegisterProperty<bool>(p => p.IsCancelled);

        /// <summary>
        /// Gets or sets the reason if order is cancelled
        /// </summary>
        /// <value>
        /// The reason.
        /// </value>
        public string CancelReason
        {
            get { return GetProperty(CancelReasonProperty); }
            set { SetProperty(CancelReasonProperty, value); }
        }
        public static readonly PropertyInfo<string> CancelReasonProperty = RegisterProperty<string>(p => p.CancelReason);

        /// <summary>
        /// Gets or sets the product list in specific order.
        /// </summary>
        /// <value>
        /// The product list.
        /// </value>
        public List<ReportOrderItem> OrderItemList
        {
            get { return _orderItemList; }
            set { _orderItemList = value; NotifyPropertyChanged(() => OrderItemList); }
        }
        /*public OrderItemList OrderItems
        {
            get
            {
                if (!FieldManager.FieldExists(OrderItemsProperty))
                {
                    LoadProperty(OrderItemsProperty, DataPortal.CreateChild<OrderItemList>());
                    OnPropertyChanged(OrderItemsProperty);
                }
                return GetProperty(OrderItemsProperty);
            }
            //set { SetProperty(OrderItemsProperty, value); }
        }
        public static readonly PropertyInfo<OrderItemList> OrderItemsProperty = RegisterProperty<OrderItemList>(p => p.OrderItems);*/
        #endregion

        #region Public Methods

        #endregion // Public Methods
    }
}
