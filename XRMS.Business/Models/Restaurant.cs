using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel.DataAnnotations;

using Csla;
using Csla.Rules.CommonRules;
using Csla.Serialization;

using XRMS.Libraries.BaseObjects;

namespace XRMS.Business.Models
{
    [Serializable]
    public class Restaurant : IdNameBaseObject<Restaurant>
    {
        #region Private Data Members

        #endregion // Private Data Members

        #region Constructors

        public Restaurant() : base()
        {

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the restaurant address.
        /// </summary>
        /// <value>
        /// The restaurant address.
        /// </value>
        public string Address
        {
            get { return GetProperty(AddressProperty); }
            set { SetProperty(AddressProperty, value); }
        }
        public static readonly PropertyInfo<string> AddressProperty = RegisterProperty<string>(p => p.Address);

        /// <summary>
        /// Gets or sets the restaurant phone (first).
        /// </summary>
        /// <value>
        /// The restaurant phone.
        /// </value>
        public string PhoneNumber1
        {
            get { return GetProperty(PhoneNumber1Property); }
            set { SetProperty(PhoneNumber1Property, value); }
        }
        public static readonly PropertyInfo<string> PhoneNumber1Property = RegisterProperty<string>(p => p.PhoneNumber1);

        /// <summary>
        /// Gets or sets the restaurant phone (second).
        /// </summary>
        /// <value>
        /// The restaurant phone.
        /// </value>
        public string PhoneNumber2
        {
            get { return GetProperty(PhoneNumber2Property); }
            set { SetProperty(PhoneNumber2Property, value); }
        }
        public static readonly PropertyInfo<string> PhoneNumber2Property = RegisterProperty<string>(p => p.PhoneNumber2);

        /// <summary>
        /// Gets or sets the restaurant phone (third).
        /// </summary>
        /// <value>
        /// The restaurant phone.
        /// </value>
        public string PhoneNumber3
        {
            get { return GetProperty(PhoneNumber3Property); }
            set { SetProperty(PhoneNumber3Property, value); }
        }
        public static readonly PropertyInfo<string> PhoneNumber3Property = RegisterProperty<string>(p => p.PhoneNumber3);

        /// <summary>
        /// Gets or sets the restaurant website.
        /// </summary>
        /// <value>
        /// The restaurant website.
        /// </value>
        public string Website
        {
            get { return GetProperty(WebsiteProperty); }
            set { SetProperty(WebsiteProperty, value); }
        }
        public static readonly PropertyInfo<string> WebsiteProperty = RegisterProperty<string>(p => p.Website);

        /// <summary>
        /// Gets or sets the restaurant email.
        /// </summary>
        /// <value>
        /// The restaurant email.
        /// </value>
        public string Email
        {
            get { return GetProperty(EmailProperty); }
            set { SetProperty(EmailProperty, value); }
        }
        public static readonly PropertyInfo<string> EmailProperty = RegisterProperty<string>(p => p.Email);


        /// <summary>
        /// Gets or sets the restaurant logo.
        /// </summary>
        /// <value>
        /// The restaurant logo.
        /// </value>
        public BitmapImage Logo
        {
            get { return GetProperty(LogoProperty); }
            set { SetProperty(LogoProperty, value); }
        }
        public static readonly PropertyInfo<BitmapImage> LogoProperty = RegisterProperty<BitmapImage>(p => p.Logo);


        /// <summary>
        /// Gets or sets the restaurant logo.
        /// </summary>
        /// <value>
        /// The restaurant logo.
        /// </value>
        public string LogoPath
        {
            get { return GetProperty(LogoPathProperty); }
            set { SetProperty(LogoPathProperty, value); }
        }
        public static readonly PropertyInfo<string> LogoPathProperty = RegisterProperty<string>(p => p.LogoPath);

        #endregion

        #region Public Methods

        #endregion
    }
}
