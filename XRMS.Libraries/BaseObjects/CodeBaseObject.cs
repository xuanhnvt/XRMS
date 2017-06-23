using System;

using Csla;

using XRMS.Libraries.CslaBase;

namespace XRMS.Libraries.BaseObjects
{
    [Serializable]
    public class CodeBaseObject<T> : CslaBusinessBase<T>, ICodeObject where T : CodeBaseObject<T>
    {
        /// <summary>
        /// Gets or sets the object code.
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        public static PropertyInfo<string> CodeProperty = RegisterProperty<string>(p => p.Code);
    }
}
