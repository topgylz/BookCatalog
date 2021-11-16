using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookCatalog.ActionFilters
{
    public class ValidateAuthorForBookExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        public ValidateAuthorForBookExistsAttribute(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = (method.Equals("PUT") || method.Equals("PATCH")) ? true : false;
            var bookId = (Guid)context.ActionArguments["bookId"];
            var book = await _repository.Book.GetBookAsync(bookId, false);
            if (book == null)
            {
                _logger.LogInfo($"Book with id: {bookId} doesn't exist in the database.");
                context.Result = new NotFoundResult();
                return;
            }
            var id = (Guid)context.ActionArguments["id"];
            var author = await _repository.Author.GetAuthorAsync(bookId, id, trackChanges);
            if (author == null)
            {
                _logger.LogInfo($"Author with id: {id} doesn't exist in the database.");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("author", author);
                await next();
            }
        }
    }
}
