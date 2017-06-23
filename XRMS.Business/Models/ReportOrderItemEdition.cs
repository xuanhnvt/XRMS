using System;

using Csla;

using XRMS.Libraries.CslaBase;

namespace XRMS.Business.Models
{
    [Serializable]
    public class ReportOrderItemEdition : CslaBusinessBase<ReportOrderItemEdition>
    {

        #region Private Data Members

        ReportOrderEdition _orderEdition;

        #endregion // Private Data Members

        #region Constructors

        public ReportOrderItemEdition() : base()
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
        /// Gets or sets the edition counter.
        /// </summary>
        /// <value>
        /// The edition counter.
        /// </value>
        public int EditionCounter
        {
            get { return GetProperty(EditionCounterProperty); }
            set { SetProperty(EditionCounterProperty, value); }
        }
        public static readonly PropertyInfo<int> EditionCounterProperty = RegisterProperty<int>(p => p.EditionCounter);

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
        /// Gets or sets the editted quantity of product.
        /// </summary>
        /// <value>
        /// The editted quantity of product.
        /// </value>
        public int EdittedQuantity
        {
            get { return GetProperty(EdittedQuantityProperty); }
            set { SetProperty(EdittedQuantityProperty, value); }
        }
        public static readonly PropertyInfo<int> EdittedQuantityProperty = RegisterProperty<int>(p => p.EdittedQuantity);

        /// <summary>
        /// Gets or sets the edition type of item.
        /// </summary>
        /// <value>
        /// The edition type of item.
        /// </value>
        public byte EditionType
        {
            get { return GetProperty(EditionTypeProperty); }
            set { SetProperty(EditionTypeProperty, value); }
        }
        public static readonly PropertyInfo<byte> EditionTypeProperty = RegisterProperty<byte>(p => p.EditionType);

        /// <summary>
        /// Gets or sets the edition info of item: collect info: counter, date, user into this property
        /// </summary>
        /// <value>
        /// The info of item.
        /// </value>
        public string EditionInfo
        {
            get {
                if (OrderEdition.EditionCounter == 0)
                    _editionInfo = Convert.ToInt32(OrderEdition.EditionCounter) + ": " + Convert.ToString(OrderEdition.EditionDate) + " - " + Convert.ToString(OrderEdition.EditionUser) + " created";
                else
                    _editionInfo = Convert.ToInt32(OrderEdition.EditionCounter) + ": " + Convert.ToString(OrderEdition.EditionDate) + " - " + Convert.ToString(OrderEdition.EditionUser) + " editted";

                return _editionInfo;
            }
        }
        string _editionInfo;

        /// <summary>
        /// Gets or sets the order edition.
        /// </summary>
        /// <value>
        /// The order edition.
        /// </value>
        public ReportOrderEdition OrderEdition
        {
            get { return _orderEdition; }
            set { _orderEdition = value; NotifyPropertyChanged(() => OrderEdition); NotifyPropertyChanged(() => EditionInfo); }
        }
        #endregion
    }
}
