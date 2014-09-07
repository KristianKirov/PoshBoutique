using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.Data.Models
{
    public class ArticleModel
    {
        public int Id { get; set; }
        public string UrlName { get; set; }
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string MaterialDescription { get; set; }
        public string ThumbnailUrl { get; set; }
        public int SizeTypeId { get; set; }
        public bool Visible { get; set; }
        public decimal? OriginalPrice { get; set; }
        public string DiscountDescription { get; set; }
        public bool HasDiscount { get; set; }
    }
}
