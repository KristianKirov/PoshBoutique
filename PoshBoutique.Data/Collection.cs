//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PoshBoutique.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Collection
    {
        public Collection()
        {
            this.Articles = new HashSet<Article>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public System.DateTime DateCreated { get; set; }
    
        public virtual ICollection<Article> Articles { get; set; }
    }
}