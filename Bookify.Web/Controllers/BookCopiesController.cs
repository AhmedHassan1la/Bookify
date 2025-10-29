using Bookify.Application.Services.BookCopies;
using Bookify.Domain.Entities;
using Bookify.Web.Core.ViewModels.BookCopy;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Web.Controllers
{
    public class BookCopiesController : Controller
    {
        private readonly IBookCopyService _bookCopyService;

        public BookCopiesController(IBookCopyService bookCopyService)
        {
            _bookCopyService = bookCopyService;
        }

        [HttpGet]
        public IActionResult Create(int bookId)
        {
            var existing = _bookCopyService.GetByBookId(bookId).FirstOrDefault();
            if (existing == null)
                return NotFound();

            var model = new BookCopyViewModel
            {
                BookId = bookId,
                IsAvailableForRental = existing.IsAvailableForRental,
                CreatedOn = DateTime.Now,
                LastUpdatedOn = DateTime.Now
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(BookCopyViewModel model)
        {
            if (model.EditionNumber < 1)
                ModelState.AddModelError("EditionNumber", "Edition Number must be >= 1.");

            if (!ModelState.IsValid)
                return View(model);

            if (_bookCopyService.EditionExists(model.BookId, model.EditionNumber))
            {
                ModelState.AddModelError("EditionNumber", "This edition already exists.");
                return View(model);
            }

            var bookCopy = new BookCopy
            {
                BookId = model.BookId,
                EditionNumber = model.EditionNumber,
                IsAvailableForRental = model.IsAvailableForRental,
                IsDeleted = model.IsDeleted,
                CreatedOn = model.CreatedOn,
                LastUpdatedOn = model.LastUpdatedOn
            };

            _bookCopyService.Add(bookCopy);

            return RedirectToAction("Details", "Books", new { id = model.BookId });
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var bookCopy = _bookCopyService.GetById(id);
            if (bookCopy == null)
                return NotFound();

            var model = new BookCopyViewModel
            {
                Id = bookCopy.Id,
                BookId = bookCopy.BookId,
                EditionNumber = bookCopy.EditionNumber,
                IsAvailableForRental = bookCopy.IsAvailableForRental,
                IsDeleted = bookCopy.IsDeleted,
                CreatedOn = bookCopy.CreatedOn,
                LastUpdatedOn = bookCopy.LastUpdatedOn
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(BookCopyViewModel model)
        {
            if (model.EditionNumber < 1)
                ModelState.AddModelError("EditionNumber", "Edition Number must be >= 1.");

            if (!ModelState.IsValid)
                return View(model);

            if (_bookCopyService.EditionExists(model.BookId, model.EditionNumber, model.Id))
            {
                ModelState.AddModelError("EditionNumber", "This edition already exists.");
                return View(model);
            }

            var bookCopy = _bookCopyService.GetById(model.Id);
            if (bookCopy == null)
                return NotFound();

            bookCopy.EditionNumber = model.EditionNumber;
            bookCopy.IsAvailableForRental = model.IsAvailableForRental;
            bookCopy.IsDeleted = model.IsDeleted;
            bookCopy.LastUpdatedOn = DateTime.Now;

            _bookCopyService.Update(bookCopy);

            return RedirectToAction("Details", "Books", new { id = model.BookId });
        }
    }
}
