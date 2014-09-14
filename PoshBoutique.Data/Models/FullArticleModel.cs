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

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string MaterialDescription { get; set; }

        public IEnumerable<SizeModel> Sizes { get; set; }

        public IEnumerable<ImageModel> Images { get; set; }
    }
}
