//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BE_WebNovel.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Review
    {
        public int review_id { get; set; }
        public Nullable<int> book_id { get; set; }
        public Nullable<int> user_id { get; set; }
        public Nullable<int> rating { get; set; }
        public string review_text { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
    
        public virtual Book Book { get; set; }
        public virtual User User { get; set; }
    }
}
