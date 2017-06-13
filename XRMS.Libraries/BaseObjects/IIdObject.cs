using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRMS.Libraries.BaseObjects
{
    public interface IIdObject
    {
        /// <summary>
        /// Gets or sets the object id.
        /// </summary>
        int Id { get; set; }
    }
}
