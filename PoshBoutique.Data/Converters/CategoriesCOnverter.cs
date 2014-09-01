using PoshBoutique.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.Data.Converters
{
    public class CategoriesConverter
    {
        public CategoryModel ToModel(Category category)
        {
            if (category == null)
            {
                return new CategoryModel()
                {
                    Id = -1,
                    OrderIndex = -1,
                    ParentCategoryId = null,
                    Title = "Всички",
                    UrlName = "all"
                };
            }

            return new CategoryModel()
            {
                Id = category.Id,
                OrderIndex = category.OrderIndex,
                ParentCategoryId = category.ParentCategoryId,
                Title = category.Title,
                UrlName = category.UrlName
            };
        }
    }
}
