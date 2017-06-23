using System;

using Csla;

using XRMS.Libraries.CslaBase;

namespace XRMS.Business.Models
{
    [Serializable]
    public class ReportOrderEdition : CslaBusinessBase<ReportOrderEdition>
    {

        #region Private Data Members

        #endregion // Private Data Members

        #region Constructors

        public ReportOrderEdition() : base()
        {

        }

        #endregion // Constructors

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
        /// Gets or sets the edition counter.
        /// </summary>
        /// <value>
        /// The edition counter.
        /// </value>
        public int EditionCounter
        {
            get { return GetProperty(EditionCounterProperty); }
            set { SetProperty(EditionCounterProperty, value); }
        }
        public static readonly PropertyInfo<int> EditionCounterProperty = RegisterProperty<int>(p => p.EditionCounter);

        /// <summary>
        /// Gets or sets the edition date.
        /// </summary>
        /// <value>
        /// The edition date.
        /// </value>
        public DateTime EditionDate
        {
            get { return GetProperty(EditionDateProperty); }
            set { SetProperty(EditionDateProperty, value); }
        }
        public static readonly PropertyInfo<DateTime> EditionDateProperty = RegisterProperty<DateTime>(p => p.EditionDate);

        /// <summary>
        /// Gets or sets the edition user.
        /// </summary>
        /// <value>
        /// The edition user.
        /// </value>
        public string EditionUser
        {
            get { return GetProperty(EditionUserProperty); }
            set { SetProperty(EditionUserProperty, value); }
        }
        public static readonly PropertyInfo<string> EditionUserProperty = RegisterProperty<string>(p => p.EditionUser);
        #endregion // Public Properties
    }
}
