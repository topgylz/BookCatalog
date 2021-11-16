using AutoMapper;
using BookCatalog.ActionFilters;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookCatalog.Controllers
{
    [Route("api/books/{bookId}/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IDataShaper<AuthorDto> _dataShaper;
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public AuthorsController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IDataShaper<AuthorDto> dataShaper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _dataShaper = dataShaper;
        }
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetAuthorsForBook(Guid bookId, [FromQuery] AuthorParameters authorParameters)
        {
            var book = await _repository.Book.GetBookAsync(bookId, trackChanges: false);
            if (book == null)
            {
                _logger.LogInfo($"Book with id: {bookId} doesn't exist in the database.");
            return NotFound();
            }
            var authorsFromDb = await _repository.Author.GetAuthorsAsync(bookId, authorParameters, trackChanges: false);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(authorsFromDb.MetaData));
            var authorsDto = _mapper.Map<IEnumerable<AuthorDto>>(authorsFromDb);
            return Ok(_dataShaper.ShapeData(authorsDto, authorParameters.Fields));
        }
        [HttpGet("{id}", Name = "GetAuthorForBook")]
        public async Task<IActionResult> GetAuthorForBook(Guid bookId, Guid id)
        {
            var book = await _repository.Book.GetBookAsync(bookId, trackChanges: false);
            if (book == null)
            {
                _logger.LogInfo($"Book with id: {bookId} doesn't exist in the database.");
            return NotFound();
            }
            var authorDb = await _repository.Author.GetAuthorAsync(bookId, id, trackChanges : false);
            if (authorDb == null)
            {
                _logger.LogInfo($"Author with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var author = _mapper.Map<AuthorDto>(authorDb);
            return Ok(author);
        }
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateAuthorForBook(Guid bookId, [FromBody]AuthorForCreationDto author)
        {
            if (author == null)
            {
                _logger.LogError("AuthorForCreationDto object sent from client is null.");
                return BadRequest("AuthorForCreationDto object is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the AuthorForCreationDto object");
                return UnprocessableEntity(ModelState);
            }
            var book = await _repository.Book.GetBookAsync(bookId, trackChanges: false);
            if (book == null)
            {
                _logger.LogInfo($"Book with id: {bookId} doesn't exist in the database.");
            return NotFound();
            }
            var authorEntity = _mapper.Map<Author>(author);
            _repository.Author.CreateAuthorForBook(bookId, authorEntity);
            await _repository.SaveAsync();
            var authorToReturn = _mapper.Map<AuthorDto>(authorEntity);
            return CreatedAtRoute("GetAuthorForBook", new
            {
                bookId,
                id = authorToReturn.Id
            }, authorToReturn);
        }
        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateAuthorForBookExistsAttribute))]
        public async Task<IActionResult> DeleteAuthorForBook(Guid bookId, Guid id)
        {
            var authorForBook= HttpContext.Items["author"] as Author;
            _repository.Author.DeleteAuthor(authorForBook);
            await _repository.SaveAsync();
            return NoContent();
        }
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateAuthorForBookExistsAttribute))]
        public async Task<IActionResult> UpdateAuthorForBook(Guid bookId, Guid id, [FromBody] AuthorForUpdateDto author)
        {
            var authorEntity = HttpContext.Items["author"] as Author;
            _mapper.Map(author, authorEntity);
            await _repository.SaveAsync();
            return NoContent();
        }
        [HttpPatch("{id}")]
        [ServiceFilter(typeof(ValidateAuthorForBookExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateAuthorForBook(Guid bookId, Guid id, [FromBody] JsonPatchDocument<AuthorForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }
            var authorEntity = HttpContext.Items["author"] as Author;
            var authorToPatch = _mapper.Map<AuthorForUpdateDto>(authorEntity);
            patchDoc.ApplyTo(authorToPatch, ModelState);
            TryValidateModel(authorToPatch);
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }
            _mapper.Map(authorToPatch, authorEntity);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}
