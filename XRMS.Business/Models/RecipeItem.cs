using System;

using Csla;
using XRMS.Libraries.CslaBase;

namespace XRMS.Business.Models
{
    [Serializable]
    public class RecipeItem : CslaBusinessBase<RecipeItem>
    {
        #region Private Data Members

        #endregion // Private Data Members

        #region Constructors

        public RecipeItem() : base()
        {

        }

        #endregion

        #region Public Properties

        public static readonly PropertyInfo<int> ProductIdProperty = RegisterProperty<int>(p => p.ProductId);
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

        public static readonly PropertyInfo<int> MaterialIdProperty = RegisterProperty<int>(p => p.MaterialId);
        /// <summary>
        /// Gets or sets the material id.
        /// </summary>
        /// <value>
        /// The material id.
        /// </value>
        public int MaterialId
        {
            get { return GetProperty(MaterialIdProperty); }
            set { SetProperty(MaterialIdProperty, value); }
        }

        public static readonly PropertyInfo<byte> SequenceProperty = RegisterProperty<byte>(p => p.Sequence);
        /// <summary>
        /// Gets or sets the sequence of material in product.
        /// </summary>
        /// <value>
        /// The sequence.
        /// </value>
        public byte Sequence
        {
            get { return GetProperty(SequenceProperty); }
            set { SetProperty(SequenceProperty, value); }
        }
        
        public static readonly PropertyInfo<decimal> UsedAmountProperty = RegisterProperty<decimal>(p => p.UsedAmount);
        /// <summary>
        /// Gets or sets the amount used in product.
        /// </summary>
        /// <value>
        /// The material amount.
        /// </value>
        public decimal UsedAmount
        {
            get { return GetProperty(UsedAmountProperty); }
            set { SetProperty(UsedAmountProperty, value); }
        }

        /// <summary>
        /// Gets or sets the material information.
        /// </summary>
        /// <value>
        /// The material information.
        /// </value>
        public Material MaterialInfo { get; set; }

        #endregion
    }

    [Serializable]
    public class RecipeItemList : CslaBusinessListBase<RecipeItemList, RecipeItem>
    {
        #region Private Data Members

        #endregion // Private Data Members

        #region Constructors

        public RecipeItemList() : base()
        {

        }

        #endregion

        #region Public Properties

        #endregion
    }
}
