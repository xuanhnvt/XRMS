using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using Csla;
using Csla.Rules.CommonRules;
using Csla.Serialization;

using XRMS.Libraries.BaseObjects;

namespace XRMS.Business.Models
{
    [Serializable]
    public class User : IdNameBaseObject<User>
    {
        #region Private Data Members

        UserRole _role;

        #endregion // Private Data Members

        #region Constructors

        public User() : base()
        {
            
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        public string Fullname
        {
            get { return GetProperty(FullnameProperty); }
            set { SetProperty(FullnameProperty, value); }
        }
        public static readonly PropertyInfo<string> FullnameProperty = RegisterProperty<string>(p => p.Fullname);

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password
        {
            get { return GetProperty(PasswordProperty); }
            set { SetProperty(PasswordProperty, value); }
        }
        public static readonly PropertyInfo<string> PasswordProperty = RegisterProperty<string>(p => p.Password);

        /// <summary>
        /// Gets or sets the role id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public byte RoleId
        {
            get { return GetProperty(RoleIdProperty); }
            set { SetProperty(RoleIdProperty, value); }
        }
        public static readonly PropertyInfo<byte> RoleIdProperty = RegisterProperty<byte>(p => p.RoleId);

        /// <summary>
        /// Gets or sets the role of user.
        /// </summary>
        /// <value>
        /// The role of user.
        /// </value>
        public UserRole Role
        {
            get { return _role; }
            set { _role = value; NotifyPropertyChanged(() => Role); }
        }

        #endregion



        #region Public Methods

        /// <summary>
        /// Do creating order, add creator information into order item
        /// </summary>
        /// <returns>Order with available creator information</returns>
        public Order CreateOrder()
        {
            Order order = Order.NewObject();
            order.CreatorId = this.Id;
            //item.CreatorUser = this;

            return order;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool StartEditOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            // order is locked by someone
            if (order.LockState == true)
                return false;
            order.LockState = true;
            order.LockKeeperId = this.Id;
            order.LockKeeper = this;

            return true;
        }
        #endregion
    }
}
