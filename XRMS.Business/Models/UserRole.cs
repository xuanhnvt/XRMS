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
    public enum Role { Unknown, Manager, Cashier, Kitchen, Waiter };

    [Serializable]
    public class UserRole : CslaBusinessBase<UserRole>
    {

        #region Private Data Members

        #endregion // Private Data Members

        #region Constructors

        public UserRole() : base()
        {

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public byte Id
        {
            get { return GetProperty(IdProperty); }
            set { SetProperty(IdProperty, value); }
        }
        public static readonly PropertyInfo<byte> IdProperty = RegisterProperty<byte>(p => p.Id);

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(p => p.Name);

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
        
        #endregion
    }
}
