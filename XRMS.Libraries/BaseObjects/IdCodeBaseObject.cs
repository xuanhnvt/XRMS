using System;
using System.ComponentModel.DataAnnotations;

using Csla;
using Csla.Rules.CommonRules;

namespace XRMS.Libraries.BaseObjects
{
    [Serializable]
    public class IdCodeBaseObject<T> : IdBaseObject<T>, ICodeObject where T : IdCodeBaseObject<T>
    {
        /// <summary>
        /// Gets or sets the object code.
        /// </summary>
        //[Required(ErrorMessage = "Please input Code")]
        //[MaxLength(10, ErrorMessage ="Code length must be equal or less than 10")]
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        public static readonly PropertyInfo<string> CodeProperty = RegisterProperty<string>(p => p.Code);

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();
            BusinessRules.AddRule(new Required(CodeProperty));
            BusinessRules.AddRule(new MaxLength(CodeProperty, 10));
        }
    }
}
