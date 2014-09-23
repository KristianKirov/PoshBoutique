﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PoshBoutiqueData : DbContext
    {
        public PoshBoutiqueData()
            : base("name=PoshBoutiqueData")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }
        public virtual DbSet<SizeType> SizeTypes { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<ArticleImage> ArticleImages { get; set; }
        public virtual DbSet<UserLike> UserLikes { get; set; }
        public virtual DbSet<EmailSubscription> EmailSubscriptions { get; set; }
        public virtual DbSet<FeedbackSubmission> FeedbackSubmissions { get; set; }
        public virtual DbSet<Collection> Collections { get; set; }
    }
}
