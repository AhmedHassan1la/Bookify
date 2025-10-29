using Bookify.Application.Common.Interfaces;
using Bookify.Application.Services.BookCopies;
using Bookify.Domain.Entities;

namespace Bookify.Application.Services.BookCopies
{
    public class BookCopyService : IBookCopyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookCopyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<BookCopy> GetByBookId(int bookId)
        {
            return _unitOfWork.BookCopies.GetAll().Where(b => b.BookId == bookId);
        }

        public bool EditionExists(int bookId, int editionNumber, int? excludeId = null)
        {
            return _unitOfWork.BookCopies.GetAll().Any(b =>
                b.BookId == bookId &&
                b.EditionNumber == editionNumber &&
                (!excludeId.HasValue || b.Id != excludeId.Value));
        }


        public BookCopy? GetById(int id)
        {
            return _unitOfWork.BookCopies.GetById(id);
        }

     

        public void Add(BookCopy bookCopy)
        {
            _unitOfWork.BookCopies.Add(bookCopy);
            _unitOfWork.Complete();
        }

        public void Update(BookCopy bookCopy)
        {
            _unitOfWork.BookCopies.Update(bookCopy);
            _unitOfWork.Complete();
        }
    }
}
