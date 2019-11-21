using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Library.API.Contexts;
using Library.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Services
{
    public class AuthorRepository : IAuthorRepository, IDisposable
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;

        public AuthorRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AuthorExistsAsync(Guid authorId)
        {
            return await _context.Authors.AnyAsync(a => a.Id == authorId);
        }

        public async Task<IEnumerable<Author>> GetAuthorsAsync()
        {
            return await _context.Authors.ToListAsync();
        }

        public async Task<Author> GetAuthorAsync(Guid authorId)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentException(nameof(authorId));
            }

            return await _context.Authors
                .FirstOrDefaultAsync(a => a.Id == authorId);
        }

        public void AddAuthor(Author author)
        {
            if(author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            _context.Authors.Add(author);
        }

        public async Task UpdateAuthor(Author author)
        {
            var authorInDb = await _context.Authors.SingleOrDefaultAsync(a => a.Id == author.Id);

            if (authorInDb == null)
                throw new ArgumentOutOfRangeException();

            authorInDb.FirstName = author.FirstName;
            _mapper.Map(author, authorInDb);

            _context.Authors.Update(authorInDb);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }
    }
}
