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
    
    public partial class BookCategory
    {
        public int BookCategory_id { get; set; }
        public Nullable<int> book_id { get; set; }
        public Nullable<int> category_id { get; set; }
        public string category_note { get; set; }
    
        public virtual Book Book { get; set; }
        public virtual Category Category { get; set; }
    }
}
