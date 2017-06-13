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
    public class ReportEvent : CslaBusinessBase<ReportEvent>
    {

        #region Private Data Members

        #endregion // Private Data Members

        #region Constructors

        public ReportEvent() : base()
        {

        }

        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the report counter.
        /// </summary>
        /// <value>
        /// The report counter.
        /// </value>
        public long ReportCounter
        {
            get { return GetProperty(ReportCounterProperty); }
            set { SetProperty(ReportCounterProperty, value); }
        }
        public static readonly PropertyInfo<long> ReportCounterProperty = RegisterProperty<long>(p => p.ReportCounter);

        /// <summary>
        /// Gets or sets the event counter.
        /// </summary>
        /// <value>
        /// The event counter.
        /// </value>
        public int EventCounter
        {
            get { return GetProperty(EventCounterProperty); }
            set { SetProperty(EventCounterProperty, value); }
        }
        public static readonly PropertyInfo<int> EventCounterProperty = RegisterProperty<int>(p => p.EventCounter);

        /// <summary>
        /// Gets or sets the event date.
        /// </summary>
        /// <value>
        /// The event date.
        /// </value>
        public DateTime EventDate
        {
            get { return GetProperty(EventDateProperty); }
            set { SetProperty(EventDateProperty, value); }
        }
        public static readonly PropertyInfo<DateTime> EventDateProperty = RegisterProperty<DateTime>(p => p.EventDate);

        /// <summary>
        /// Gets or sets the type of event.
        /// </summary>
        /// <value>
        /// The type of event.
        /// </value>
        public int EventClass
        {
            get { return GetProperty(EventClassProperty); }
            set { SetProperty(EventClassProperty, value); }
        }
        public static readonly PropertyInfo<int> EventClassProperty = RegisterProperty<int>(p => p.EventClass);

        /// <summary>
        /// Gets or sets the event text.
        /// </summary>
        /// <value>
        /// The event text.
        /// </value>
        public string Text
        {
            get { return GetProperty(TextProperty); }
            set { SetProperty(TextProperty, value); }
        }
        public static readonly PropertyInfo<string> TextProperty = RegisterProperty<string>(p => p.Text);
        #endregion
    }
}
