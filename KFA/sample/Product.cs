//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace sample
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        public Product()
        {
            this.Categories = new HashSet<Category>();
            this.Subcategories = new HashSet<Subcategory>();
        }
    
        public int Id { get; set; }
        public string PoductName { get; set; }
    
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Subcategory> Subcategories { get; set; }
    }
}