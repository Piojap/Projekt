//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BootlegSteam
{
    using System;
    using System.Collections.Generic;
    
    public partial class game
    {
        public long id { get; set; }
        public string title { get; set; }
        public System.DateTime creation { get; set; }
        public long score { get; set; }
        public byte[] picture { get; set; }
        public long devid { get; set; }
    
        public virtual dev dev { get; set; }
    }
}
