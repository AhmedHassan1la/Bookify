
// IUnitOfWork.cs
using Bookify.Application.Common.Interfaces.Repositories;
using Bookify.Domain.Entities;
using System.Linq;

namespace Bookify.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IBaseRepository<Author> Authors { get; }
        IBaseRepository<Category> Categories { get; }
        IBaseRepository<Book> Books { get; }
        IBaseRepository<BookCopy> BookCopies { get; }
        IBaseRepository<Rental> Rentals { get; }
        IBaseRepository<RentalBookCopy> RentalBookCopies { get; }
        IBaseRepository<Subscriber> Subscribers { get; }

        IQueryable<Subscriber> SubscribersQueryable { get; }
        IQueryable<BookCopy> BookCopiesQueryable { get; }
        IQueryable<RentalBookCopy> RentalBookCopiesQueryable { get; }

        int Complete();
    }
}
