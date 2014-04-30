using PoshBoutique.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.Data.Extensions
{
    public static class QueruableExtensions
    {
        public static IQueryable<Article> Sort(this IQueryable<Article> articleQuery, string orderBy, SortDirection sortDirection)
        {
            orderBy = orderBy.ToUpperInvariant();
            switch (orderBy)
            {
                case "DATE":
                    if (sortDirection == SortDirection.ASC)
                    {
                        articleQuery = articleQuery.OrderBy(a => a.DateCreated);
                    }
                    else
                    {
                        articleQuery = articleQuery.OrderByDescending(a => a.DateCreated);
                    }
                    break;
                case "TITLE":
                    if (sortDirection == SortDirection.ASC)
                    {
                        articleQuery = articleQuery.OrderBy(a => a.Title);
                    }
                    else
                    {
                        articleQuery = articleQuery.OrderByDescending(a => a.Title);
                    }
                    break;
                case "PRICE":
                    if (sortDirection == SortDirection.ASC)
                    {
                        articleQuery = articleQuery.OrderBy(a => a.Price);
                    }
                    else
                    {
                        articleQuery = articleQuery.OrderByDescending(a => a.Price);
                    }
                    break;
                case "LIKES":
                    break;
                case "ORDERS":
                    break;
            }

            return articleQuery;
        }
    }
}
