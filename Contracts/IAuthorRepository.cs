using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IAuthorRepository
    {
        Task<PagedList<Author>> GetAuthorsAsync(Guid companyId, AuthorParameters authorParameters, bool trackChanges);
        Task<Author> GetAuthorAsync(Guid bookId, Guid id, bool trackChanges);
        void CreateAuthorForBook(Guid bookId, Author author);
        void DeleteAuthor(Author author);
    }
}
