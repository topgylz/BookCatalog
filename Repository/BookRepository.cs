using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public async Task<Book> GetBookAsync(Guid bookId, bool trackChanges) =>
            await FindByCondition(b => b.Id.Equals(bookId), trackChanges).SingleOrDefaultAsync();
        public BookRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
        public async Task<IEnumerable<Book>> GetAllBooksAsync(bool trackChanges) =>
            await FindAll(trackChanges).OrderBy(b => b.Name).ToListAsync();
        public void CreateBook(Book book) => Create(book);
        public async Task<IEnumerable<Book>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(x => ids.Contains(x.Id), trackChanges).ToListAsync();
        public void DeleteBook(Book book)
        {
            Delete(book);
        }
    }
}
