using Bookify.Application.Services.Rentals;
using Bookify.Contracts.ViewModels.Rental;
using Bookify.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Web.Controllers
{
    public class RentalsController : Controller
    {
        private readonly IRentalService _rentalService;

        public RentalsController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int subscriberId)
        {
            var rentalCount = await _rentalService.GetCurrentRentalCountAsync(subscriberId);
            ViewData["SubscriberHasMaxRentals"] = rentalCount >= 3;

            return View(new CreateRentalViewModel
            {
                SubscriberId = subscriberId
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRentalViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var canRent = await _rentalService.CanRentAsync(model.SubscriberId);
            if (!canRent)
            {
                ModelState.AddModelError("", "This subscriber is not allowed to rent books.");
                return View(model);
            }

            var validSerials = model.SerialNumbers?.Where(x => x > 0).ToList() ?? new();
            if (!validSerials.Any())
            {
                ModelState.AddModelError("", "Please enter at least one valid Serial Number.");
                return View(model);
            }

            var rentedBooks = await _rentalService.GetAlreadyRentedBooksAsync(validSerials);
            if (rentedBooks.Any())
            {
                ViewData["RentedBooks"] = rentedBooks;
                ModelState.AddModelError("", "Some of the selected books are already rented.");
                return View(model);
            }

            var availableCopies = await _rentalService.GetAvailableBookCopiesAsync(validSerials);
            if (!availableCopies.Any())
            {
                ModelState.AddModelError("", "No available books found.");
                return View(model);
            }

            var rental = await _rentalService.RentBooksAsync(model.SubscriberId, availableCopies);
            return RedirectToAction(nameof(RentedBookCopies), new { subscriberId = model.SubscriberId });
        }

        [HttpGet]
        public async Task<IActionResult> RentedBookCopies(int subscriberId)
        {
            var rentedBooks = await _rentalService.GetRentedBooksAsync(subscriberId);
            return View(rentedBooks);
        }

        [HttpPost]
        public async Task<IActionResult> ReturnBook(int serialNumber)
        {
            var success = await _rentalService.ReturnBookAsync(serialNumber);
            if (!success)
            {
                return Json(new { success = false, message = "Book not found or already returned." });
            }

            return Json(new { success = true, returnDate = DateTime.Today.ToString("dd/MM/yyyy") });
        }

        [HttpGet]
        public async Task<IActionResult> SearchBookCopies([FromQuery] List<int> serialNumbers)
        {
            var result = await _rentalService.GetAvailableBookCopiesAsync(serialNumbers);
            var data = result.Select(bc => new
            {
                serialNumber = bc.SerialNumber,
                bookTitle = bc.Book.Title,
                editionNumber = bc.EditionNumber
            });

            return Json(data);
        }
    }
}
