using System;
using System.ComponentModel.DataAnnotations;

using Csla;
using Csla.Rules.CommonRules;
using XRMS.Libraries.BaseObjects;

namespace XRMS.Business.Models
{
    [Serializable]
    public class Area : IdCodeNameBaseObject<Area>
    {
        #region Private Data Members

        #endregion // Private Data Members

        #region Constructors

        public Area() : base()
        {

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        public static readonly PropertyInfo<string> DescriptionProperty = RegisterProperty<string>(p => p.Description);

        #endregion
    }
}
