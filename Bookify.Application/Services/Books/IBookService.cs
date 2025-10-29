using Bookify.Domain.Entities;

namespace Bookify.Application.Services.Books
{
    public interface IBookService
    {
        IEnumerable<Book> GetAllWithDetails();
        IEnumerable<Book> GetAll();
        Book? GetByIdWithDetails(int id);
        Book? GetById(int id);
        void Add(Book book);
        void Update(Book book);
        void Complete();
    }
}
