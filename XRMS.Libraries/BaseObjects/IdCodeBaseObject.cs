using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
