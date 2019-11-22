using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.API.Entities;

namespace Library.API.Services
{
    public interface IAuthorRepository : IDisposable
    {
        Task<bool> AuthorExistsAsync(int authorId);

        Task<IEnumerable<Author>> GetAuthorsAsync();

        Task<Author> GetAuthorAsync(int authorId);

        Task UpdateAuthor(Author author);

        Task<bool> SaveChangesAsync();
    }
}
