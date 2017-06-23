using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace XRMS.Libraries.MVVM
{
    /// <summary>
    /// Base class for entities that need to notify their property change event
    /// It implements the INotifyPropertyChanged interface
    /// </summary>
    public class PropertyChangedNotifierBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// To be called when the value of the property, whose name is passed as parameter, changes.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public virtual void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// To be called when the value of the property, whose selector is passed as parameter, changes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expressiong "bringing" the property.</param>
        public virtual void NotifyPropertyChanged<T>(Expression<Func<T>> expression)
        {
            string propertyName = ((MemberExpression)expression.Body).Member.Name;

            if (string.IsNullOrEmpty(propertyName) == false)
                NotifyPropertyChanged(propertyName);
        }
    }
}
