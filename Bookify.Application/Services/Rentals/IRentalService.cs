using Bookify.Contracts.ViewModels.Rental;
using Bookify.Domain.Entities;


namespace Bookify.Application.Services.Rentals
{
    public interface IRentalService
    {
        Task<bool> CanRentAsync(int subscriberId);
        Task<int> GetCurrentRentalCountAsync(int subscriberId);
        Task<IEnumerable<RentedBookViewModel>> GetRentedBooksAsync(int subscriberId);
        Task<IEnumerable<RentedBookViewModel>> GetAlreadyRentedBooksAsync(List<int> serialNumbers);
        Task<List<BookCopy>> GetAvailableBookCopiesAsync(List<int> serialNumbers);
        Task<Rental> RentBooksAsync(int subscriberId, List<BookCopy> bookCopies);
        Task<bool> ReturnBookAsync(int serialNumber);
    }
}
