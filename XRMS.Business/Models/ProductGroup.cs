using System;

using Csla;
using XRMS.Libraries.BaseObjects;

namespace XRMS.Business.Models
{
    [Serializable]
    public class ProductGroup : IdCodeNameBaseObject<ProductGroup>
    {
        #region Private Data Members

        #endregion // Private Data Members

        #region Constructors

        public ProductGroup() : base()
        {
            //MarkNew();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public static readonly PropertyInfo<string> DescriptionProperty = RegisterProperty<string>(p => p.Description);
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }

        #endregion
    }
}
