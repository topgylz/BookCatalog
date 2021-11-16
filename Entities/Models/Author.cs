using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Models
{
    public class Author
    {
        [Column("AuthorId")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Фамилия автора - обязательное поле.")]
        public string Surname { get; set; }
        [ForeignKey(nameof(Book))]
        public Guid BookId { get; set; }
        public Book Book { get; set; }
    }
}
