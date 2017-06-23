using System;

using Csla;

namespace XRMS.Libraries.BaseObjects
{
    [Serializable]
    public class IdNameBaseObject<T> : IdBaseObject<T> where T : IdNameBaseObject<T>
    {
        /// <summary>
        /// Gets or sets the object name.
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        public static PropertyInfo<string> NameProperty = RegisterProperty<string>(p => p.Name);
    }
}
