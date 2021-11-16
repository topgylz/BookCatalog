using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Configuration
{
    public class AuthorConfiguration: IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasData
            (
            new Author
            {
                Id = new Guid("80abbca8-664d-4b20-b5de-024705497d4a"),
                Surname = "Гоголь",
                BookId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870")
            },
            new Author
            {
                Id = new Guid("86dba8c0-d178-41e7-938c-ed49778fb52a"),
                Surname = "Пушкин",
                BookId = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3")
            }
            );
        }
    }
}
