
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
    
public partial class TableEntity
{

    public int Id { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public byte Capacity { get; set; }

    public int LocationId { get; set; }

    public string Description { get; set; }

    public byte State { get; set; }

    public long CurrentOrderId { get; set; }



    public virtual AreaEntity Area { get; set; }

}

}
