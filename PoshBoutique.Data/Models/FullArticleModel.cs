using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.Data.Models
{
    public class FullArticleModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string UrlName { get; set; }

        public decimal? OriginalPrice { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string ShortDescription { get; set; }

        public string MaterialDescription { get; set; }

        public IEnumerable<SizeModel> Sizes { get; set; }

        public IEnumerable<ImageModel> Images { get; set; }

        public string ThumbnailUrl { get; set; }

        public bool IsLiked { get; set; }

        public bool HasDiscount { get; set; }

        public string DiscountDescription { get; set; }
    }
}
