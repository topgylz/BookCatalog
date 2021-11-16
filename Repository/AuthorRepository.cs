using Contracts;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Extensions;

namespace Repository
{
    public class AuthorRepository : RepositoryBase<Author>, IAuthorRepository
    {
        public async Task<PagedList<Author>> GetAuthorsAsync(Guid bookId, AuthorParameters authorParameters, bool trackChanges)
        {
            var authors = await FindByCondition(a => a.BookId.Equals(bookId), trackChanges).Search(authorParameters.SearchTerm).Sort(authorParameters.OrderBy).ToListAsync();
            return PagedList<Author>.ToPagedList(authors, authorParameters.PageNumber, authorParameters.PageSize);
        }
        public async Task<Author> GetAuthorAsync(Guid bookId, Guid id, bool trackChanges) =>
            await FindByCondition(a => a.BookId.Equals(bookId) && a.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
        public AuthorRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
        public void CreateAuthorForBook(Guid bookId, Author author)
        {
            author.BookId = bookId;
            Create(author);
        }
        public void DeleteAuthor(Author author)
        {
            Delete(author);
        }
    }
}
