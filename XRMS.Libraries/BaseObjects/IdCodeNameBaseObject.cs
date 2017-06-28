using System;
using System.ComponentModel.DataAnnotations;

using Csla;
using Csla.Rules.CommonRules;

namespace XRMS.Libraries.BaseObjects
{
    [Serializable]
    public class IdCodeNameBaseObject<T> : IdCodeBaseObject<T> where T : IdCodeNameBaseObject<T>
    {
        /// <summary>
        /// Gets or sets the object name.
        /// </summary>
        //[Required(ErrorMessage = "Please input Name")]
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(p => p.Name);

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();
            BusinessRules.AddRule(new Required(NameProperty));
        }
    }
}
