// DashboardController.cs (Using IUnitOfWork)
using Bookify.Application.Common.Interfaces;
using Bookify.Web.Core.ViewModels.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel
            {
                TotalBooks = _unitOfWork.Books.GetAll().Count(),
                TotalBookCopies = _unitOfWork.BookCopies.GetAll().Count(),
                TotalSubscribers = _unitOfWork.Subscribers.GetAll().Count(),
                ActiveSubscriptions = _unitOfWork.SubscribersQueryable.SelectMany(s => s.Subscriptions!).Count(s => s.EndDate >= DateTime.Today),
                AvailableBookCopies = _unitOfWork.BookCopiesQueryable.Count(bc => bc.IsAvailableForRental),
                RentedBookCopies = _unitOfWork.BookCopiesQueryable.Count(bc => !bc.IsAvailableForRental),
                ActiveRentals = _unitOfWork.Rentals.GetAll().Count(r => r.StartDate <= DateTime.Today && r.RentalBookCopies.Any(rb => rb.ReturnDate == null)),
                BlacklistedSubscribers = _unitOfWork.SubscribersQueryable.Count(s => s.IsBlackListed),
                ExpiredSubscriptions = _unitOfWork.SubscribersQueryable.SelectMany(s => s.Subscriptions!).Count(s => s.EndDate < DateTime.Today),
                RentalsLast30Days = _unitOfWork.Rentals.GetAll().Count(r => r.StartDate >= DateTime.Today.AddDays(-30)),
                ReturnedBooksLast30Days = _unitOfWork.Rentals.GetAll().Count(r => r.RentalBookCopies.Any(rb => rb.ReturnDate >= DateTime.Today.AddDays(-30))),

                TopRentedBooks = _unitOfWork.RentalBookCopiesQueryable
                    .Include(rb => rb.BookCopy)
                        .ThenInclude(bc => bc!.Book)
                    .AsEnumerable()
                    .GroupBy(rb => rb.BookCopy!.Book!.Title)
                    .OrderByDescending(g => g.Count())
                    .Take(5)
                    .Select(g => new TopBookViewModel
                    {
                        Title = g.Key,
                        RentalCount = g.Count()
                    })
                    .ToList(),

                MonthlyRentals = _unitOfWork.Rentals.GetAll()
                    .AsEnumerable()
                    .GroupBy(r => new { r.StartDate.Year, r.StartDate.Month })
                    .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                    .ToDictionary(
                        g => $"{g.Key.Year}-{g.Key.Month:D2}",
                        g => g.Count()
                    ),

                CategoryRentals = _unitOfWork.Books
                     .GetAllWithIncludes(b => b.Categories)
                     .AsEnumerable()
                     .SelectMany(b => b.Categories, (b, c) => new { b, c })
                     .GroupBy(bc => bc.c.Name)
                     .OrderByDescending(g => g.Count())
                     .ToDictionary(g => g.Key,g => g.Count() )

            };

            return View(viewModel);
        }
    }
}
