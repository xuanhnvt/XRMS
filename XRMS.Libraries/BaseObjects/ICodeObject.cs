using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRMS.Libraries.BaseObjects
{
    public interface ICodeObject
    {
        /// <summary>
        /// Gets or sets the object code.
        /// </summary>
        string Code { get; set; }
    }
}
