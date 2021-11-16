using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects
{
    public abstract class AuthorForManipulationDto
    {
        [Required(ErrorMessage = "Фамилия автора - обязательное поле.")]
        public string Surname { get; set; }
    }
}
