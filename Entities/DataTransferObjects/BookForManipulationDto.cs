using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects
{
    public abstract class BookForManipulationDto
    {
        [Required(ErrorMessage = "Название книги является обязательным полем.")]
        [MaxLength(30, ErrorMessage = "Максимальная длина названия книги - 30 символов.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Год издания книги является обязательным полем.")]
        public int Year { get; set; }
        public IEnumerable<AuthorForCreationDto> Authors { get; set; }
    }
}
