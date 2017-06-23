using System;

using Csla;
using XRMS.Libraries.BaseObjects;

namespace XRMS.Business.Models
{
    [Serializable]
    public class Material : IdCodeNameBaseObject<Material>
    {

        #region Private Data Members

        #endregion // Private Data Members

        #region Constructors

        public Material() : base()
        {
            
        }

        #endregion

        #region Public Properties
        
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        public static readonly PropertyInfo<string> DescriptionProperty = RegisterProperty<string>(p => p.Description);


        /// <summary>
        /// Gets or sets the material group id.
        /// </summary>
        /// <value>
        /// The material group id.
        /// </value>
        public int GroupId
        {
            get { return GetProperty(GroupIdProperty); }
            set { SetProperty(GroupIdProperty, value); }
        }
        public static readonly PropertyInfo<int> GroupIdProperty = RegisterProperty<int>(p => p.GroupId);

        /// <summary>
        /// Gets or sets the material unit id.
        /// </summary>
        /// <value>
        /// The material unit id.
        /// </value>
        public int UnitId
        {
            get { return GetProperty(UnitIdProperty); }
            set { SetProperty(UnitIdProperty, value); }
        }
        public static readonly PropertyInfo<int> UnitIdProperty = RegisterProperty<int>(p => p.UnitId);

        /// <summary>
        /// Gets or sets the current material amount before confirming check.
        /// </summary>
        /// <value>
        /// The material amount.
        /// </value>
        public decimal Amount
        {
            get { return GetProperty(AmountProperty); }
            set { SetProperty(AmountProperty, value); NotifyPropertyChanged(() => RemainAmount); }
        }
        public static readonly PropertyInfo<decimal> AmountProperty = RegisterProperty<decimal>(p => p.Amount);

        /// <summary>
        /// Gets or sets the current using amount.
        /// </summary>
        /// <value>
        /// The material amount.
        /// </value>
        public decimal UsageAmount
        {
            get { return GetProperty(UsageAmountProperty); }
            set { SetProperty(UsageAmountProperty, value); NotifyPropertyChanged(() => RemainAmount); }
        }
        public static readonly PropertyInfo<decimal> UsageAmountProperty = RegisterProperty<decimal>(p => p.UsageAmount);

        /// <summary>
        /// Gets or sets the last material amount at last confirming.
        /// </summary>
        /// <value>
        /// The material amount.
        /// </value>
        public decimal LastConfirmationAmount
        {
            get { return GetProperty(LastConfirmationAmountProperty); }
            set { SetProperty(LastConfirmationAmountProperty, value); }
        }
        public static readonly PropertyInfo<decimal> LastConfirmationAmountProperty = RegisterProperty<decimal>(p => p.LastConfirmationAmount);

        /// <summary>
        /// Gets or sets the last datetime when user do operation of confirming.
        /// </summary>
        /// <value>
        /// The confirming datetime.
        /// </value>
        public DateTime ConfirmationDate
        {
            get { return GetProperty(ConfirmationDateProperty); }
            set { SetProperty(ConfirmationDateProperty, value); }
        }
        public static readonly PropertyInfo<DateTime> ConfirmationDateProperty = RegisterProperty<DateTime>(p => p.ConfirmationDate);

        /// <summary>
        /// Gets or sets the remaining amount.
        /// </summary>
        /// <value>
        /// The material amount.
        /// </value>
        public decimal RemainAmount
        {
            get { return Amount - UsageAmount; }
        }

        /// <summary>
        /// Gets or sets the material unit.
        /// </summary>
        /// <value>
        /// The material unit.
        /// </value>
        public Unit Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }
        private Unit _unit;

        /// <summary>
        /// Gets or sets the material group.
        /// </summary>
        /// <value>
        /// The material group.
        /// </value>
        public MaterialGroup Group
        {
            get { return _group; }
            set { _group = value; }
        }
        private MaterialGroup _group;

        #endregion
    }
}
