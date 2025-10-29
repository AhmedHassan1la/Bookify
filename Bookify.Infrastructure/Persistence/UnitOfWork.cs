// UnitOfWork.cs
using Bookify.Application.Common.Interfaces;
using Bookify.Application.Common.Interfaces.Repositories;
using Bookify.Domain.Entities;
using Bookify.Infrastructure.Persistence;
using Bookify.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IApplicationDbContext _context;

        public IBaseRepository<Author> Authors { get; }
        public IBaseRepository<Category> Categories { get; }
        public IBaseRepository<Book> Books { get; }
        public IBaseRepository<BookCopy> BookCopies { get; }
        public IBaseRepository<Rental> Rentals { get; }
        public IBaseRepository<RentalBookCopy> RentalBookCopies { get; }
        public IBaseRepository<Subscriber> Subscribers { get; }

        public IQueryable<Subscriber> SubscribersQueryable => _context.Subscribers.AsQueryable();
        public IQueryable<BookCopy> BookCopiesQueryable => _context.BookCopies.Include(b => b.Book).AsQueryable();
        public IQueryable<RentalBookCopy> RentalBookCopiesQueryable => _context.RentalBookCopies.AsQueryable();

        public UnitOfWork(IApplicationDbContext context)
        {
            _context = context;
            Authors = new BaseRepository<Author>(_context);
            Categories = new BaseRepository<Category>(_context);
            Books = new BaseRepository<Book>(_context);
            BookCopies = new BaseRepository<BookCopy>(_context);
            Rentals = new BaseRepository<Rental>(_context);
            RentalBookCopies = new BaseRepository<RentalBookCopy>(_context);
            Subscribers = new BaseRepository<Subscriber>(_context);
        }

        public int Complete() => _context.SaveChanges();
    }
}