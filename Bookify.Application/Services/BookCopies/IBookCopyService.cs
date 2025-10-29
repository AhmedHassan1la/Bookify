using Bookify.Domain.Entities;

namespace Bookify.Application.Services.BookCopies
{
    public interface IBookCopyService
    {
        IEnumerable<BookCopy> GetByBookId(int bookId);
        BookCopy? GetById(int id);
        bool EditionExists(int bookId, int editionNumber, int? excludeId = null);
        void Add(BookCopy bookCopy);
        void Update(BookCopy bookCopy);
    }
}
