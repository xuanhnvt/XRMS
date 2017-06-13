using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using Csla;
using Csla.Rules.CommonRules;
using Csla.Serialization;

using XRMS.Libraries.Constants;
using XRMS.Libraries.CslaBase;

namespace XRMS.Business.Models
{
    public enum OrderItemState { Ordered, Processing, Ready, Served };

    [Serializable]
    public class OrderItem : CslaBusinessBase<OrderItem>
    {
        #region Private Data Members

        int _edittedQuantity;
        int _oldQuantity;

        #endregion // Private Data Members

        #region Constructors

        public OrderItem() : base()
        {
            // set default time
            this.CreateDatetime = Convert.ToDateTime(DateTimeFormats.YMDHMS_NULLDATE);
            this.StartDatetime = Convert.ToDateTime(DateTimeFormats.YMDHMS_NULLDATE);
            this.StopDatetime = Convert.ToDateTime(DateTimeFormats.YMDHMS_NULLDATE);
            this.ServeDatetime = Convert.ToDateTime(DateTimeFormats.YMDHMS_NULLDATE);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the order id.
        /// </summary>
        /// <value>
        /// The order id.
        /// </value>
        public long OrderId
        {
            get { return GetProperty(OrderIdProperty); }
            set { SetProperty(OrderIdProperty, value); }
        }
        public static readonly PropertyInfo<long> OrderIdProperty = RegisterProperty<long>(p => p.OrderId);

        /// <summary>
        /// Gets or sets the product id.
        /// </summary>
        /// <value>
        /// The product id.
        /// </value>
        public int ProductId
        {
            get { return GetProperty(ProductIdProperty); }
            set { SetProperty(ProductIdProperty, value); }
        }
        public static readonly PropertyInfo<int> ProductIdProperty = RegisterProperty<int>(p => p.ProductId);

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
        /// Gets or sets the quantity of product.
        /// </summary>
        /// <value>
        /// The quantity of product.
        /// </value>
        public int Quantity
        {
            get { return GetProperty(QuantityProperty); }
            set
            {
                SetProperty(QuantityProperty, value);
                _edittedQuantity = Quantity - OldQuantity;
                NotifyPropertyChanged(() => EdittedQuantity);
                NotifyPropertyChanged(() => ItemPrice);
            }
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
            private set { SetProperty(IsCancelledProperty, value); }
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
            get { return Quantity * ProductInfo.Price; }
        }

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        public Product ProductInfo
        {
            get { return _productInfo; }
            set { _productInfo = value; NotifyPropertyChanged(() => ProductInfo); NotifyPropertyChanged(() => ItemPrice); }
        }
        private Product _productInfo;

        /// <summary>
        /// Gets or sets the editted quantity of item.
        /// </summary>
        /// <value>
        /// The editted quantity.
        /// </value>
        public int EdittedQuantity
        {
            get { return _edittedQuantity; }
            set
            {
                _edittedQuantity = value;
                SetProperty(QuantityProperty, _edittedQuantity + OldQuantity);
                NotifyPropertyChanged(() => EdittedQuantity);
            }
        }

        /// <summary>
        /// Gets or sets the old quantity of item.
        /// </summary>
        /// <value>
        /// The updated quantity.
        /// </value>
        public int OldQuantity
        {
            get { return _oldQuantity; }
        }
        #endregion

        #region Public Methods

        public void SetOldQuantity()
        {
            _oldQuantity = this.Quantity;
            NotifyPropertyChanged(() => OldQuantity);
        }

        public void MarkCancelled()
        {
            IsCancelled = true;
            _edittedQuantity = -Quantity;
        }

        public void UnMarkCancelled()
        {
            IsCancelled = false;
        }

        #endregion
    }

    [Serializable]
    public class OrderItemList : CslaBusinessListBase<OrderItemList, OrderItem>
    {
        #region Private Data Members

        #endregion // Private Data Members

        #region Constructors

        public OrderItemList() : base()
        {

        }

        #endregion

        #region Public Properties

        #endregion
    }
}
