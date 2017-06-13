using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using Csla;
using Csla.Rules.CommonRules;
using Csla.Serialization;

using Cinch;
using XRMS.Libraries.BaseObjects;

namespace XRMS.Business.Models
{
    [Serializable]
    public class Unit : IdCodeNameBaseObject<Unit>
    {
        #region Private Data Members

        #endregion // Private Data Members

        #region Constructors

        public Unit() : base()
        {
            //MarkNew();
        }

        #endregion

        #region Public Properties
        
        #endregion
    }
}
