using Entities.Models;
using Repository.Extensions.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace Repository.Extensions
{
    public static class RepositoryAuthorExtensions
    {
        public static IQueryable<Author> Search(this IQueryable<Author> authors, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return authors;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return authors.Where(a => a.Surname.ToLower().Contains(lowerCaseTerm));
        }
        public static IQueryable<Author> Sort(this IQueryable<Author> authors, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return authors.OrderBy(a => a.Surname);
            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Author>(orderByQueryString);
            if (string.IsNullOrWhiteSpace(orderQuery))
                return authors.OrderBy(a => a.Surname);
            return authors.OrderBy(orderQuery);
        }
    }
}
