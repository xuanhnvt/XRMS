using System;

namespace XRMS.Libraries.CslaBase
{
    [Serializable]
    public abstract class CslaBusinessListBase<T, C> : Csla.BusinessListBase<T, C> where T : CslaBusinessListBase<T, C> where C : Csla.Core.IEditableBusinessObject
    {

    }
}
