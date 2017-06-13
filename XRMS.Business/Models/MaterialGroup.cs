using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using Csla;
using Csla.Rules.CommonRules;
using Csla.Serialization;

using Cinch;
using XRMS.Libraries.BaseObjects;

namespace XRMS.Business.Models
{
    [Serializable]
    public class MaterialGroup : IdCodeNameBaseObject<MaterialGroup>
    {
        #region Private Data Members

        #endregion // Private Data Members

        #region Constructors

        public MaterialGroup() : base()
        {

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
