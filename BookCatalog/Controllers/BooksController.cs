using AutoMapper;
using BookCatalog.ActionFilters;
using BookCatalog.ModelBinders;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookCatalog.Controllers
{
    [Route("api/books")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class BooksController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public BooksController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        /// <summary>
        /// Gets the list of all books
        /// </summary>
        /// <returns>The books list</returns>
        [HttpGet(Name = "GetBooks"), Authorize]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _repository.Book.GetAllBooksAsync(trackChanges: false);
            var booksDto = _mapper.Map<IEnumerable<BookDto>>(books);
            return Ok(booksDto);
        }
        [HttpGet("{id}", Name = "BookById")]
        public async Task<IActionResult> GetBook(Guid id)
        {
            var book = await _repository.Book.GetBookAsync(id, trackChanges: false);
            if (book == null)
            {
                _logger.LogInfo($"Book with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            else
            {
                var bookDto = _mapper.Map<BookDto>(book);
                return Ok(bookDto);
            }
        }
        /// <summary>
        /// Creates a newly created book
        /// </summary>
        /// <param name="book"></param>
        /// <returns>A newly created book</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> CreateBook([FromBody] BookForCreationDto book)
        {
            if (book == null)
            {
                _logger.LogError("BookForCreationDto object sent from client is null.");
                return BadRequest("BookForCreationDto object is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the BookForCreationDto object");
                return UnprocessableEntity(ModelState);
            }
            var bookEntity = _mapper.Map<Book>(book);
            _repository.Book.CreateBook(bookEntity);
            await _repository.SaveAsync();
            var bookToReturn = _mapper.Map<BookDto>(bookEntity);
            return CreatedAtRoute("BookById", new { id = bookToReturn.Id }, bookToReturn);
        }
        [HttpGet("collection/({ids})", Name = "BookCollection")]
        public async Task<IActionResult> GetBookCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }
            var bookEntities = await _repository.Book.GetByIdsAsync(ids, trackChanges: false);
            if (ids.Count() != bookEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }
            var booksToReturn = _mapper.Map<IEnumerable<BookDto>>(bookEntities);
            return Ok(booksToReturn);
        }
        [HttpPost("collection")]
        public async Task<IActionResult> CreateBookCollection([FromBody] IEnumerable<BookForCreationDto> bookCollection)
        {
            if (bookCollection == null)
            {
                _logger.LogError("Book collection sent from client is null.");
                return BadRequest("Book collection is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the BookForUpdateDto object");
                return UnprocessableEntity(ModelState);
            }
            var bookEntities = _mapper.Map<IEnumerable<Book>>(bookCollection);
            foreach (var book in bookEntities)
            {
                _repository.Book.CreateBook(book);
            }
            await _repository.SaveAsync();
            var bookCollectionToReturn =
            _mapper.Map<IEnumerable<BookDto>>(bookEntities);
            var ids = string.Join(",", bookCollectionToReturn.Select(b => b.Id));
            return CreatedAtRoute("BookCollection", new { ids }, bookCollectionToReturn);
        }
        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateBookExistsAttribute))]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            var book = HttpContext.Items["book"] as Book;
            _repository.Book.DeleteBook(book);
            await _repository.SaveAsync();
            return NoContent();
        }
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateBookExistsAttribute))]
        public async Task<IActionResult> UpdateBook(Guid id, [FromBody] BookForUpdateDto book)
        {
            var bookEntity = HttpContext.Items["book"] as Book;
            _mapper.Map(book, bookEntity);
            await _repository.SaveAsync();
            return NoContent();
        }
        [HttpOptions]
        public IActionResult GetBooksOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");
            return Ok();
        }
    }
}
