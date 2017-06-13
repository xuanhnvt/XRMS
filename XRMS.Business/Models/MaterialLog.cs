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
using XRMS.Libraries.Constants;

namespace XRMS.Business.Models
{
    [Serializable]
    public class MaterialLog : CslaBusinessBase<MaterialLog>
    {

        #region Private Data Members

        #endregion // Private Data Members

        #region Constructors

        public MaterialLog() : base()
        {

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the log counter.
        /// </summary>
        /// <value>
        /// The log counter.
        /// </value>
        public long LogCounter
        {
            get { return GetProperty(LogCounterProperty); }
            set { SetProperty(LogCounterProperty, value); }
        }
        public static readonly PropertyInfo<long> LogCounterProperty = RegisterProperty<long>(p => p.LogCounter);

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
        public static readonly PropertyInfo<int> MaterialIdProperty = RegisterProperty<int>(p => p.MaterialId);

        /// <summary>
        /// Gets or sets the actor name that do action.
        /// </summary>
        /// <value>
        /// The actor name.
        /// </value>
        public string Actor
        {
            get { return GetProperty(ActorProperty); }
            set { SetProperty(ActorProperty, value); }
        }
        public static readonly PropertyInfo<string> ActorProperty = RegisterProperty<string>(p => p.Actor);

        /// <summary>
        /// Gets or sets the action type: confirming, adding, correcting.
        /// </summary>
        /// <value>
        /// The action type.
        /// </value>
        public byte Action
        {
            get { return GetProperty(ActionProperty); }
            set { SetProperty(ActionProperty, value); }
        }
        public static readonly PropertyInfo<byte> ActionProperty = RegisterProperty<byte>(p => p.Action);


        /// <summary>
        /// Gets or sets the material amount before action is performed.
        /// </summary>
        /// <value>
        /// The material amount.
        /// </value>
        public decimal CurrentAmount
        {
            get { return GetProperty(CurrentAmountProperty); }
            set { SetProperty(CurrentAmountProperty, value); NotifyPropertyChanged(() => AffectedAmount); }
        }
        public static readonly PropertyInfo<decimal> CurrentAmountProperty = RegisterProperty<decimal>(p => p.CurrentAmount);

        /// <summary>
        /// Gets or sets the updated amount after action is performing.
        /// </summary>
        /// <value>
        /// The material amount.
        /// </value>
        public decimal NewAmount
        {
            get { return GetProperty(NewAmountProperty); }
            set { SetProperty(NewAmountProperty, value); NotifyPropertyChanged(() => AffectedAmount); }
        }
        public static readonly PropertyInfo<decimal> NewAmountProperty = RegisterProperty<decimal>(p => p.NewAmount);

        /// <summary>
        /// Gets or sets the log datetime when user do action.
        /// </summary>
        /// <value>
        /// The log datetime.
        /// </value>
        public DateTime LogDatetime
        {
            get { return GetProperty(LogDatetimeProperty); }
            set { SetProperty(LogDatetimeProperty, value); }
        }
        public static readonly PropertyInfo<DateTime> LogDatetimeProperty = RegisterProperty<DateTime>(p => p.LogDatetime);

        /// <summary>
        /// Gets or sets the affected amount after action is performing.
        /// </summary>
        /// <value>
        /// The material amount.
        /// </value>
        public decimal AffectedAmount
        {
            get { return NewAmount - CurrentAmount; }
        }
        #endregion
    }
}
