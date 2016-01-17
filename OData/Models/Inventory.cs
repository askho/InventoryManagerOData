//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OData.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Inventory
    {
        public int inventory_id { get; set; }
        public int item_id { get; set; }
        public System.DateTime date_added { get; set; }
        public decimal condition { get; set; }
        public Nullable<decimal> price { get; set; }
        public Nullable<int> company_id { get; set; }
        public Nullable<int> event_id { get; set; }
        public Nullable<int> member_id { get; set; }
        public Nullable<System.DateTime> date_checked_out { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual Event Event { get; set; }
        public virtual Member Member { get; set; }
        public virtual Item Item { get; set; }
    }
}
