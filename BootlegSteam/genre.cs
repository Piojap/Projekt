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
    
    public partial class genre
    {
        public genre()
        {
            this.games = new HashSet<game>();
        }
    
        public long id { get; set; }
        public string title { get; set; }
    
        public virtual ICollection<game> games { get; set; }
    }
}