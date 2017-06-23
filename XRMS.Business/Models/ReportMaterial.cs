using System;

using Csla;

using XRMS.Libraries.CslaBase;

namespace XRMS.Business.Models
{
    [Serializable]
    public class ReportMaterial : CslaBusinessBase<ReportMaterial>
    {

        #region Private Data Members

        #endregion // Private Data Members

        #region Constructors

        public ReportMaterial() : base()
        {

        }

        #endregion // Constructors

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
        /// Gets or sets the order item sequence of order report.
        /// </summary>
        /// <value>
        /// The sequence.
        /// </value>
        public byte OrderItemSequence
        {
            get { return GetProperty(OrderItemSequenceProperty); }
            set { SetProperty(OrderItemSequenceProperty, value); }
        }
        public static readonly PropertyInfo<byte> OrderItemSequenceProperty = RegisterProperty<byte>(p => p.OrderItemSequence);

        /// <summary>
        /// Gets or sets the material code.
        /// </summary>
        /// <value>
        /// The material code.
        /// </value>
        public string MaterialCode
        {
            get { return GetProperty(MaterialCodeProperty); }
            set { SetProperty(MaterialCodeProperty, value); }
        }
        public static readonly PropertyInfo<string> MaterialCodeProperty = RegisterProperty<string>(p => p.MaterialCode);

        /// <summary>
        /// Gets or sets the material name.
        /// </summary>
        /// <value>
        /// The material name.
        /// </value>
        public string MaterialName
        {
            get { return GetProperty(MaterialNameProperty); }
            set { SetProperty(MaterialNameProperty, value); }
        }
        public static readonly PropertyInfo<string> MaterialNameProperty = RegisterProperty<string>(p => p.MaterialName);

        /// <summary>
        /// Gets or sets the material amount that used by product.
        /// </summary>
        /// <value>
        /// The material amount.
        /// </value>
        public decimal Amount
        {
            get { return GetProperty(AmountProperty); }
            set { SetProperty(AmountProperty, value); }
        }
        public static readonly PropertyInfo<decimal> AmountProperty = RegisterProperty<decimal>(p => p.Amount);

        /// <summary>
        /// Gets or sets the unit name of material report.
        /// </summary>
        /// <value>
        /// The unit name.
        /// </value>
        public string UnitName
        {
            get { return GetProperty(UnitNameProperty); }
            set { SetProperty(UnitNameProperty, value); }
        }
        public static readonly PropertyInfo<string> UnitNameProperty = RegisterProperty<string>(p => p.UnitName);

        #endregion // Public Properties
    }
}
