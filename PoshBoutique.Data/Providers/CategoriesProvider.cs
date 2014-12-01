using PoshBoutique.Data.Converters;
using PoshBoutique.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.Data.Providers
{
    public class CategoriesProvider
    {
        public IEnumerable<CategoryModel> GetCategoriesTree()
        {
            List<CategoryModel> categoriesTree = new List<CategoryModel>();

            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                CategoriesConverter converter = new CategoriesConverter();
                List<CategoryModel> allCategories = dataContext.Categories.OrderBy(c => c.OrderIndex).
                    ToList().
                    Select(c => converter.ToModel(c)).
                    ToList();
                Dictionary<int, CategoryModel> categoriesDictionary = allCategories.ToDictionary(c => c.Id);

                foreach (CategoryModel categoryModel in allCategories)
                {
                    if (categoryModel.ParentCategoryId == null)
                    {
                        categoriesTree.Add(categoryModel);
                    }
                    else
                    {
                        int parentCategoryId = categoryModel.ParentCategoryId.Value;
                        CategoryModel parentCategory = categoriesDictionary[parentCategoryId];
                        parentCategory.ChildCategories.Add(categoryModel);
                    }
                }
            }

            return categoriesTree;
        }
    }
}
