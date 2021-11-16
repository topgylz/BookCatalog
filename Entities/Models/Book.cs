using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Models
{
    public class Book
    {
        [Column("BookId")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Название книги является обязательным полем.")]
        [MaxLength(30, ErrorMessage = "Максимальная длина названия книги - 30 символов.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Год издания книги является обязательным полем.")]
        public int Year { get; set; }
        public ICollection<Author> Authors { get; set; }
    }
}
