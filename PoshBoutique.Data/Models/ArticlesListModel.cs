using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.Data.Models
{
    public class ArticlesListModel
    {
        public CategoryModel Category { get; set; }

        public IList<ArticleModel> Articles { get; set; }
    }
}
