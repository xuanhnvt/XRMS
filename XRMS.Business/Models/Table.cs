using System;
using System.ComponentModel.DataAnnotations;

using Csla;
using Csla.Rules.CommonRules;
using XRMS.Libraries.BaseObjects;

namespace XRMS.Business.Models
{
    public enum TableState { Free, Busy, Billed };

    [Serializable]
    public class Table : IdCodeNameBaseObject<Table>
    {
        #region Private Data Members

        #endregion // Private Data Members

        #region Constructors

        public Table() : base()
        {

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [Required(ErrorMessage = "Please input Description")]
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        public static readonly PropertyInfo<string> DescriptionProperty = RegisterProperty<string>(p => p.Description);

        /// <summary>
        /// Gets or sets the capacity.
        /// </summary>
        public byte Capacity
        {
            get { return GetProperty(CapacityProperty); }
            set { SetProperty(CapacityProperty, value); }
        }
        public static readonly PropertyInfo<byte> CapacityProperty = RegisterProperty<byte>(p => p.Capacity);


        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        public TableState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        public static readonly PropertyInfo<TableState> StateProperty = RegisterProperty<TableState>(p => p.State);

        /// <summary>
        /// Gets or sets the current order id that is ordered at this table.
        /// </summary>
        public long CurrentOrderId
        {
            get { return GetProperty(CurrentOrderIdProperty); }
            set { SetProperty(CurrentOrderIdProperty, value); }
        }
        public static readonly PropertyInfo<long> CurrentOrderIdProperty = RegisterProperty<long>(p => p.CurrentOrderId);


        /// <summary>
        /// Gets or sets the id of area that this table is located at.
        /// </summary>
        public int LocationId
        {
            get { return GetProperty(LocationIdProperty); }
            set { SetProperty(LocationIdProperty, value); }
        }
        public static readonly PropertyInfo<int> LocationIdProperty = RegisterProperty<int>(p => p.LocationId);


        /// <summary>
        /// Gets or sets the area location where table is located in.
        /// </summary>
        public Area Location
        {
            get { return _location; }
            set { _location = value; }
        }
        private Area _location;

        #endregion
    }
}
