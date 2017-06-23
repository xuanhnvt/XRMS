using System;
using System.Linq.Expressions;

namespace XRMS.Libraries.CslaBase
{
    //public abstract class CslaBusinessBase : Cinch.EditableValidatingObject
    [Serializable]
    public abstract class CslaBusinessBase<T> : Csla.CustomFieldData.BusinessCore<T> where T : CslaBusinessBase<T>
    {
        #region Private Data Members

        // Is this object can be edited or not
        //private bool _isEditable = true;

        // Is object new or has it been loaded from database or saved to it?
        //private bool _isNew = true;

        // By keeping track of whether this object is currently being edited or not
        // it can make sure that the object's data is only changed when appropriate
        //private bool _isEditing = false;

        // Has object's data been changed?
        //private bool _isDirty = true;

        // Has object been marked for deletion?
        //private bool _isDeleted = false;

        // Has object been updated from database?
        private bool _isUpdated = false;

        // Has object acknownledged updated data from database?
        private bool _isAcknownledged = false;
        

        #endregion // Private Data Members

        #region Public Properties

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        //public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// To be called when the value of the property, whose name is passed as parameter, changes.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /*public virtual void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }*/

        /// <summary>
        /// To be called when the value of the property, whose selector is passed as parameter, changes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expressiong "bringing" the property.</param>
        public virtual void NotifyPropertyChanged<TObject>(Expression<Func<TObject>> expression)
        {
            string propertyName = ((MemberExpression)expression.Body).Member.Name;

            if (string.IsNullOrEmpty(propertyName) == false)
                base.OnPropertyChanged(propertyName);
        }

        /*/// <summary>
        /// Is this object can be edited or not?
        /// </summary>
        public bool IsEditable
        {
            get { return _isEditable; }
            set { _isEditable = value; }
        }*/

        /// <summary>
        /// Returns true if this is a new object, 
        /// false if it is a pre-existing object.
        /// </summary>
        /// <remarks>
        /// An object is considered to be new if its primary identifying (key) value 
        /// doesn't correspond to data in the database. In other words, 
        /// if the data values in this particular
        /// object have not yet been saved to the database the object is considered to
        /// be new. Likewise, if the object's data has been deleted from the database
        /// then the object is considered to be new.
        /// </remarks>
        /// <returns>A value indicating if this object is new.</returns>
        /*public bool IsNew
        {
            get { return _isNew; }
        }*/

        /// <summary>
        /// Returns true if this object is marked for deletion.
        /// </summary>
        /// <remarks>
        /// This property is part of the support for deferred deletion, where an object
        /// can be marked for deletion, but isn't actually deleted until the object
        /// is saved to the database. This property indicates whether or not the
        /// current object has been marked for deletion. If it is true
        /// , the object will
        /// be deleted when it is saved to the database, otherwise it will be inserted
        /// or updated by the save operation.
        /// </remarks>
        /// <returns>A value indicating if this object is marked for deletion.</returns>
        /*public bool IsDeleted
        {
            get { return _isDeleted; }
        }*/

        /// <summary>
        /// Returns true if this object's 
        /// data, or any of its fields or child objects data, 
        /// has been changed.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When an object's data is changed, the object is considered to be 'dirty' or changed.
        /// This value is used to optimize data updates, since an unchanged object does not need to be
        /// updated into the database. All new objects are considered dirty. All objects
        /// marked for deletion are considered dirty.
        /// </para><para>
        /// Once an object's data has been saved to the database (inserted or updated)
        /// the dirty flag is cleared and the object is considered unchanged. Objects
        /// newly loaded from the database are also considered unchanged.
        /// </para>
        /// </remarks>
        /// <returns>A value indicating if this object's data has been changed.</returns>
        /*public virtual bool IsDirty
        {
            get { return IsSelfDirty || HasPropertyChanged(); }
        }*/

        /// <summary>
        /// Returns true if this object's data has been changed.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When an object's data is changed, the object is considered to be 'dirty' or changed.
        /// This value is used to optimize data updates, since an unchanged object does not need to be
        /// updated into the database. All new objects are considered dirty. All objects
        /// marked for deletion are considered dirty.
        /// </para><para>
        /// Once an object's data has been saved to the database (inserted or updated)
        /// the dirty flag is cleared and the object is considered unchanged. Objects
        /// newly loaded from the database are also considered unchanged.
        /// </para>
        /// </remarks>
        /// <returns>A value indicating if this object's data has been changed.</returns>
        /*public virtual bool IsSelfDirty
        {
            get { return _isDirty; }
        }*/

        /// <summary>
        /// By keeping track of whether this object is currently being edited or not?
        /// It can make sure that the object's data is only changed when appropriate
        /// </summary>
        /*public bool IsEditing
        {
            get { return _isEditing; }
            set { _isEditing = value; }
        }*/

        /// <summary>
        /// Has object been updated from database?
        /// </summary>
        public bool IsUpdated
        {
            get { return _isUpdated; }
        }

        /// <summary>
        /// Has object been updated from database?
        /// </summary>
        public bool IsAcknownledged
        {
            get { return _isAcknownledged; }
        }
        #endregion

        #region Private Method Members
        /// <summary>
        /// Deteremines if at least a property has changes since object was put into edit mode
        /// </summary>
        /// <param name="propertyName">The property name</param>
        /// <returns>True if object has changes since it was put into edit mode</returns>
        //private bool HasPropertyChanged(string propertyName)
        /*private bool HasPropertyChanged()
        {
            if (_savedState == null)
                return false;

            Dictionary<string, object> currentState = this.GetFieldValues();

            foreach (FieldInfo property in GetType().GetFields(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                Debug.WriteLine("Field Name : " + property.Name);

                object saveValue;
                object currentValue;
                if (_savedState.TryGetValue(property.Name, out saveValue) &&
                      currentState.TryGetValue(property.Name, out currentValue))
                {

                    if (property.Name != "_savedState")
                    {
                        if (saveValue == null || currentValue == null)
                        {
                            if (saveValue != currentValue)
                            {
                                Debug.WriteLine("Field Name : " + property.Name + " has null value");
                                return true;
                            }
                        }

                        if (!saveValue.Equals(currentValue))
                        {
                            Debug.WriteLine("Field Name: " + property.Name + " = " + saveValue.ToString() + " - " + currentValue.ToString());
                            return true;
                        }
                    }
                }*/
        //return saveValue != currentValue;

        //if (saveValue.Equals(currentValue))
        //return false;
        //return !saveValue.Equals(currentValue);

        /*object value;
        if (fieldValues.TryGetValue(fi.Name, out value))
            fi.SetValue(this, value);
        else
        {
            Debug.WriteLine("Failed to restore field " +
                fi.Name + " from cloned values, field not found in Dictionary.");
        }*/
        /*}
        return false;

    }*/
        #endregion // Private Method Members

        #region Public Method Members
        public static T NewObject()
        {
            T item = Csla.DataPortal.Create<T>();
            //MarkNew(item);
            return item;
        }
        /// <summary>
        /// Marks the object as being a new object. This also marks the object
        /// as being dirty and ensures that it is not marked for deletion.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Newly created objects are marked new by default. You should call
        /// this method when the object is deleted (due to being marked for deletion) to indicate
        /// that the object no longer reflects data in the database.
        /// </para><para>
        /// If you override this method, make sure to call the base
        /// implementation after executing your new code.
        /// </para>
        /// </remarks>
        /*public virtual void MarkNew()
        {
            _isNew = true;
            _isDeleted = false;
            //MetaPropertyHasChanged("IsNew");
            //MetaPropertyHasChanged("IsDeleted");
            MarkDirty();
        }*/

        /// <summary>
        /// Marks the object as being an old (not new) object. This also
        /// marks the object as being unchanged (not dirty).
        /// </summary>
        /// <remarks>
        /// <para>
        /// You should call this method to indicate that an existing object has been
        /// successfully retrieved from the database.
        /// </para><para>
        /// You should call this method to indicate that a new object has been successfully
        /// inserted into the database.
        /// </para><para>
        /// If you override this method, make sure to call the base
        /// implementation after executing your new code.
        /// </para>
        /// </remarks>
        public new void MarkOld()
        {
            //_isNew = false;
            //MetaPropertyHasChanged("IsNew");
            base.MarkOld();
        }

        public new void MarkAsChild()
        {
            //_isNew = false;
            //MetaPropertyHasChanged("IsNew");
            base.MarkAsChild();
        }

        public void MarkUpdated()
        {
            _isUpdated = true;
            _isAcknownledged = false;
            NotifyPropertyChanged(() => IsUpdated);
        }

        public virtual void Acknowledge()
        {
            _isUpdated = false;
            _isAcknownledged = true;
            NotifyPropertyChanged(() => IsUpdated);
            NotifyPropertyChanged(() => IsAcknownledged);
        }

        /// <summary>
        /// Marks an object for deletion. This also marks the object
        /// as being dirty.
        /// </summary>
        /// <remarks>
        /// You should call this method in your business logic in the
        /// case that you want to have the object deleted when it is
        /// saved to the database.
        /// </remarks>
        /*public void MarkDeleted()
        {
            _isDeleted = true;
            //MetaPropertyHasChanged("IsDeleted");
            MarkDirty();
        }*/

        /// <summary>
        /// Marks an object as being dirty, or changed.
        /// </summary>
        /// <remarks>
        /// <para>
        /// You should call this method in your business logic any time
        /// the object's internal data changes.
        /// </para><para>
        /// Marking an object as dirty does two things. First it ensures
        /// that we save the object as appropriate. Second,
        /// it tell Windows Forms data binding that the
        /// object's data has changed so any bound controls will update to
        /// reflect the new values.
        /// </para>
        /// </remarks>
        /*protected void MarkDirty()
        {
            _isDirty = true;
        }*/

        /// <summary>
        /// Forces the object's IsDirty flag to false.
        /// </summary>
        /// <remarks>
        /// This method is normally called automatically and is
        /// not intended to be called manually.
        /// </remarks>
        /*protected void MarkClean()
        {
            _isDirty = false;
        }*/
        #endregion // Protected Method Members

        #region Override Methods for base class Cinch.EditableValidatingObject
        /// <summary>
        /// This is used to clone the object.  
        /// Override the method to provide a more efficient clone.  
        /// The default implementation simply reflects across 
        /// the object copying every field.
        /// </summary>
        /// <returns>Clone of current object</returns>
        /*protected override Dictionary<string, object> GetFieldValues()
        {
            return GetType().GetFields(BindingFlags.Public |
                BindingFlags.NonPublic | BindingFlags.Instance).Select(
                fi => new { Key = fi.Name, Value = fi.GetValue(this) })
                    .ToDictionary(k => k.Key, k => k.Value);
        }*/

        /// <summary>
        /// This restores the state of the current object from the passed clone object.
        /// </summary>
        /// <param name="fieldValues">Object to restore state from</param>
        /*protected override void RestoreFieldValues(Dictionary<string, object> fieldValues)
        {
            foreach (FieldInfo fi in GetType().GetFields(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                object value;
                if (fieldValues.TryGetValue(fi.Name, out value))
                    fi.SetValue(this, value);
                else
                {
                    Debug.WriteLine("Failed to restore field " +
                        fi.Name + " from cloned values, field not found in Dictionary.");
                }
            }
        }*/

        /// <summary>
        /// Indicates that the specified property belongs
        /// to the business object type.
        /// </summary>
        /// <typeparam name="P">Type of property</typeparam>
        /// <param name="propertyLambdaExpression">Property Expression</param>
        /// <returns></returns>
        /*protected static PropertyInfo<P> RegisterProperty<T, P>(Expression<Func<T, object>> propertyLambdaExpression)
        {
            PropertyInfo reflectedPropertyInfo = Reflect<T>.GetProperty(propertyLambdaExpression);

            return RegisterProperty<P>(typeof(T), Csla.Core.FieldManager.PropertyInfoFactory.Factory.Create<P>(typeof(T), reflectedPropertyInfo.Name));
        }*/
        #endregion // Override Methods for base class Cinch.EditableValidatingObject
    }
}
