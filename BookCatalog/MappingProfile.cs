using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookCatalog
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDto>().ForMember(b => b.FullInfo,
            opt => opt.MapFrom(x => string.Join(' ', x.Name, x.Year)));
            CreateMap<Author, AuthorDto>();
            CreateMap<BookForCreationDto, Book>();
            CreateMap<AuthorForCreationDto, Author>();
            CreateMap<AuthorForUpdateDto, Author>().ReverseMap();
            CreateMap<BookForUpdateDto, Book>();
            CreateMap<UserForRegistrationDto, User>();
        }
    }
}
