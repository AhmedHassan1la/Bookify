using Bookify.Application.Common.Interfaces;
using Bookify.Domain.Entities;

namespace Bookify.Application.Services.Books
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Book> GetAll() =>
            _unitOfWork.Books.GetAll();

        public IEnumerable<Book> GetAllWithDetails() =>
            _unitOfWork.Books.GetAllWithIncludes(b => b.Author!, b => b.Categories, b => b.BookCopies);

        public Book? GetById(int id) =>
            _unitOfWork.Books.GetById(id);

        public Book? GetByIdWithDetails(int id) =>
            _unitOfWork.Books
                .GetAllWithIncludes(b => b.Author!, b => b.Categories, b => b.BookCopies)
                .FirstOrDefault(b => b.Id == id);

        public void Add(Book book)
        {
            _unitOfWork.Books.Add(book);
        }

        public void Update(Book book)
        {
            _unitOfWork.Books.Update(book);
        }

        public void Complete()
        {
            _unitOfWork.Complete();
        }
    }
}
