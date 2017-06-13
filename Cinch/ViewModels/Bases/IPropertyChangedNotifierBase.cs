#region System Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;
#endregion

namespace Cinch
{
    /// <summary>
    /// Interface for all the class that need to have NotifyPropertyChange
    /// </summary>
    public interface IPropertyChangedNotifierBase : INotifyPropertyChanged
    {
        /// <summary>
        /// To be called when the value of the property, whose name is passed as parameter, changes.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        void NotifyPropertyChanged(string propertyName);

        /// <summary>
        /// To be called when the value of the property, whose selector is passed as parameter, changes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expressiong "bringing" the property.</param>
        void NotifyPropertyChanged<T>(Expression<Func<T>> expression);
    }
}
