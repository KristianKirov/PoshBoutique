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
    
    public partial class ArticleImage
    {
        public int ArticleId { get; set; }
        public string SmallImageUrl { get; set; }
        public string MediumImageUrl { get; set; }
        public string LargelImageUrl { get; set; }
        public int OrderIndex { get; set; }
        public int Id { get; set; }
    
        public virtual Article Article { get; set; }
    }
}
