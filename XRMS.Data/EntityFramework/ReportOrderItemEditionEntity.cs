//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace XRMS.Data.EntityFramework
{
    using System;
    using System.Collections.Generic;
    
    public partial class ReportOrderItemEditionEntity
    {
        public long ReportCounter { get; set; }
        public int EditionCounter { get; set; }
        public byte Sequence { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public byte EditionType { get; set; }
        public int EdittedQuantity { get; set; }
    }
}