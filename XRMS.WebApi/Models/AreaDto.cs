using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XRMS.WebApi.Models
{
    public class AreaDto
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}