using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using Csla;
using Csla.Rules.CommonRules;
using Csla.Serialization;

using XRMS.Libraries.CslaBase;

namespace XRMS.Business.Models
{
    [Serializable]
    public class ReportOrderItem : CslaBusinessBase<ReportOrderItem>
    {
        #region Private Data Members


        #endregion // Private Data Members

        #region Constructors

        public ReportOrderItem() : base()
        {

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
        /// Gets or sets the sequence of item.
        /// </summary>
        /// <value>
        /// The sequence.
        /// </value>
        public byte Sequence
        {
            get { return GetProperty(SequenceProperty); }
            set { SetProperty(SequenceProperty, value); }
        }
        public static readonly PropertyInfo<byte> SequenceProperty = RegisterProperty<byte>(p => p.Sequence);

        /// <summary>
        /// Gets or sets the product code.
        /// </summary>
        /// <value>
        /// The product code.
        /// </value>
        public string ProductCode
        {
            get { return GetProperty(ProductCodeProperty); }
            set { SetProperty(ProductCodeProperty, value); }
        }
        public static readonly PropertyInfo<string> ProductCodeProperty = RegisterProperty<string>(p => p.ProductCode);

        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        /// <value>
        /// The product name.
        /// </value>
        public string ProductName
        {
            get { return GetProperty(ProductNameProperty); }
            set { SetProperty(ProductNameProperty, value); }
        }
        public static readonly PropertyInfo<string> ProductNameProperty = RegisterProperty<string>(p => p.ProductName);

        /// <summary>
        /// Gets or sets the product group.
        /// </summary>
        /// <value>
        /// The product group.
        /// </value>
        public string ProductGroup
        {
            get { return GetProperty(ProductGroupProperty); }
            set { SetProperty(ProductGroupProperty, value); }
        }
        public static readonly PropertyInfo<string> ProductGroupProperty = RegisterProperty<string>(p => p.ProductGroup);

        /// <summary>
        /// Gets or sets the product unit.
        /// </summary>
        /// <value>
        /// The product unit.
        /// </value>
        public string UnitName
        {
            get { return GetProperty(UnitNameProperty); }
            set { SetProperty(UnitNameProperty, value); }
        }
        public static readonly PropertyInfo<string> UnitNameProperty = RegisterProperty<string>(p => p.UnitName);

        /// <summary>
        /// Gets or sets the price of product per unit.
        /// </summary>
        /// <value>
        /// The price of product.
        /// </value>
        public decimal UnitPrice
        {
            get { return GetProperty(UnitPriceProperty); }
            set { SetProperty(UnitPriceProperty, value); }
        }
        public static readonly PropertyInfo<decimal> UnitPriceProperty = RegisterProperty<decimal>(p => p.UnitPrice);

        /// <summary>
        /// Gets or sets the quantity of product.
        /// </summary>
        /// <value>
        /// The quantity of product.
        /// </value>
        public int Quantity
        {
            get { return GetProperty(QuantityProperty); }
            set { SetProperty(QuantityProperty, value); NotifyPropertyChanged(() => ItemPrice); }
        }
        public static readonly PropertyInfo<int> QuantityProperty = RegisterProperty<int>(p => p.Quantity);

        /// <summary>
        /// Gets or sets the order creating datetime.
        /// </summary>
        /// <value>
        /// The edit datetime.
        /// </value>
        public DateTime CreateDatetime
        {
            get { return GetProperty(CreateDatetimeProperty); }
            set { SetProperty(CreateDatetimeProperty, value); }
        }
        public static readonly PropertyInfo<DateTime> CreateDatetimeProperty = RegisterProperty<DateTime>(p => p.CreateDatetime);

        public DateTime StartDatetime
        {
            get { return GetProperty(StartDatetimeProperty); }
            set { SetProperty(StartDatetimeProperty, value); }
        }
        public static readonly PropertyInfo<DateTime> StartDatetimeProperty = RegisterProperty<DateTime>(p => p.StartDatetime);

        public DateTime StopDatetime
        {
            get { return GetProperty(StopDatetimeProperty); }
            set { SetProperty(StopDatetimeProperty, value); }
        }
        public static readonly PropertyInfo<DateTime> StopDatetimeProperty = RegisterProperty<DateTime>(p => p.StopDatetime);

        public DateTime ServeDatetime
        {
            get { return GetProperty(ServeDatetimeProperty); }
            set { SetProperty(ServeDatetimeProperty, value); }
        }
        public static readonly PropertyInfo<DateTime> ServeDatetimeProperty = RegisterProperty<DateTime>(p => p.ServeDatetime);

        /// <summary>
        /// Gets or sets the state of item.
        /// </summary>
        /// <value>
        /// The state of item.
        /// </value>
        public OrderItemState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        public static readonly PropertyInfo<OrderItemState> StateProperty = RegisterProperty<OrderItemState>(p => p.State);

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
        /// Gets or sets the flag IsAlwaysReady (check if this item need to be processed in kitchen or not)
        /// </summary>
        /// <value>
        /// The flag.
        /// </value>
        public bool IsAlwaysReady
        {
            get { return GetProperty(IsAlwaysReadyProperty); }
            set { SetProperty(IsAlwaysReadyProperty, value); }
        }
        public static readonly PropertyInfo<bool> IsAlwaysReadyProperty = RegisterProperty<bool>(p => p.IsAlwaysReady);

        /// <summary>
        /// Gets or sets the price of item.
        /// </summary>
        /// <value>
        /// The price of item.
        /// </value>
        public decimal ItemPrice
        {
            get { return Quantity * UnitPrice; }
        }
        #endregion

        #region Public Methods

        #endregion
    }

    [Serializable]
    public class ReportOrderItemList : CslaBusinessListBase<ReportOrderItemList, ReportOrderItem>
    {
        #region Private Data Members

        #endregion // Private Data Members

        #region Constructors

        public ReportOrderItemList() : base()
        {

        }

        #endregion

        #region Public Properties

        #endregion
    }
}
