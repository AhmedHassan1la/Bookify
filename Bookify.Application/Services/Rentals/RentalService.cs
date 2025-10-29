using Bookify.Application.Common.Interfaces;
using Bookify.Contracts.ViewModels.Rental;
using Bookify.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Application.Services.Rentals
{
    public class RentalService : IRentalService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RentalService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CanRentAsync(int subscriberId)
        {
            var subscriber = await _unitOfWork.SubscribersQueryable
                .Where(s => s.Id == subscriberId)
                .Select(s => new { s.IsBlackListed, s.IsDeleted })
                .FirstOrDefaultAsync();

            return subscriber != null && !subscriber.IsBlackListed && !subscriber.IsDeleted;
        }

        public async Task<int> GetCurrentRentalCountAsync(int subscriberId)
        {
            return await _unitOfWork.RentalBookCopiesQueryable
                .CountAsync(r => r.Rental!.SubscriberId == subscriberId && r.ReturnDate == null);
        }

        public async Task<IEnumerable<RentedBookViewModel>> GetRentedBooksAsync(int subscriberId)
        {
            return await _unitOfWork.RentalBookCopiesQueryable
                .Include(rbc => rbc.BookCopy)
                .ThenInclude(bc => bc.Book)
                .Where(rbc => rbc.Rental!.SubscriberId == subscriberId)
                .Select(rbc => new RentedBookViewModel
                {
                    SerialNumber = rbc.BookCopy!.SerialNumber,
                    BookTitle = rbc.BookCopy.Book!.Title,
                    EditionNumber = rbc.BookCopy.EditionNumber,
                    RentalDate = rbc.RentalDate,
                    RentalEnd = rbc.RentalEnd,
                    ReturnDate = rbc.ReturnDate
                }).ToListAsync();
        }

        public async Task<IEnumerable<RentedBookViewModel>> GetAlreadyRentedBooksAsync(List<int> serialNumbers)
        {
            return await _unitOfWork.RentalBookCopiesQueryable
                .Include(rbc => rbc.BookCopy)
                .Where(rbc => serialNumbers.Contains(rbc.BookCopy!.SerialNumber) && rbc.ReturnDate == null)
                .Select(rbc => new RentedBookViewModel
                {
                    SerialNumber = rbc.BookCopy!.SerialNumber,
                    RentedBy = rbc.Rental!.SubscriberId
                }).ToListAsync();
        }

        public async Task<List<BookCopy>> GetAvailableBookCopiesAsync(List<int> serialNumbers)
        {
            return await _unitOfWork.BookCopiesQueryable
                .Where(bc => serialNumbers.Contains(bc.SerialNumber) && bc.IsAvailableForRental)
                .ToListAsync();
        }

        public async Task<Rental> RentBooksAsync(int subscriberId, List<BookCopy> bookCopies)
        {
            var rental = new Rental
            {
                SubscriberId = subscriberId,
                RentalBookCopies = bookCopies.Select(bc => new RentalBookCopy
                {
                    BookCopyId = bc.Id,
                    RentalDate = DateTime.Today
                }).ToList()
            };

            _unitOfWork.Rentals.Add(rental);

            foreach (var bc in bookCopies)
            {
                bc.IsAvailableForRental = false;
                _unitOfWork.BookCopies.Update(bc);
            }

            _unitOfWork.Complete();

            return rental;
        }

        public async Task<bool> ReturnBookAsync(int serialNumber)
        {
            var rentalBookCopy = await _unitOfWork.RentalBookCopiesQueryable
                .Include(r => r.Rental)
                .FirstOrDefaultAsync(r =>
                    r.BookCopy.SerialNumber == serialNumber &&
                    r.ReturnDate == null);

            if (rentalBookCopy == null)
                return false;

            rentalBookCopy.ReturnDate = DateTime.Today;
            _unitOfWork.RentalBookCopies.Update(rentalBookCopy);

            var bookCopy = _unitOfWork.BookCopies.FirstOrDefault(b => b.SerialNumber == serialNumber);
            if (bookCopy != null)
            {
                bookCopy.IsAvailableForRental = true;
                _unitOfWork.BookCopies.Update(bookCopy);
            }

            _unitOfWork.Complete();
            return true;
        }
    }
}
