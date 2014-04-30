using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.Data.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string UrlName { get; set; }
        public string Title { get; set; }
        public int? ParentCategoryId { get; set; }
        public int OrderIndex { get; set; }

        public virtual IList<CategoryModel> ChildCategories { get; set; }

        public CategoryModel()
        {
            this.ChildCategories = new List<CategoryModel>();
        }
    }
}
