//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Connect.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Connection
    {
        public long ConnectionId { get; set; }
        public long User_Sender { get; set; }
        public long User_Receiver { get; set; }
        public bool Active { get; set; }
    
        public virtual User Sender { get; set; }
        public virtual User Receiver { get; set; }
    }
}
