using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Csla;
using XRMS.Libraries.CslaBase;

namespace XRMS.Libraries.BaseObjects
{
    [Serializable]
    public class IdBaseObject<T> : CslaBusinessBase<T>, IIdObject where T : IdBaseObject<T>
    {
        /// <summary>
        /// Gets or sets the object id.
        /// </summary>
        public int Id
        {
            get { return GetProperty(IdProperty); }
            set { SetProperty(IdProperty, value); }
        }
        public static PropertyInfo<int> IdProperty = RegisterProperty<int>(p => p.Id);
    }
}
