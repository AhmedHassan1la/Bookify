using Bookify.Application.Services.Authors;
using Bookify.Application.Services.Books;
using Bookify.Application.Services.Categories;
using Bookify.Domain.Entities;
using Bookify.Web.Core.ViewModels.Book;
using Bookify.Web.Core.ViewModels.BookCopy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Packaging;

namespace Bookify.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IAuthorService _authorService;
        private readonly ICategoryService _categoryService;

        public BooksController(IBookService bookService, IAuthorService authorService, ICategoryService categoryService)
        {
            _bookService = bookService;
            _authorService = authorService;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            var books = _bookService.GetAllWithDetails()
                .Where(b => !b.IsDeleted)
                .Select(b => new BookViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    IsAvailableForRental = b.IsAvailableForRental,
                    Hall = b.Hall,
                    Publisher = b.Publisher,
                    PublishingDate = b.PublishingDate,
                    ImageUrl = b.ImageUrl,
                    Description = b.Description,
                    SelectedAuthorId = b.AuthorId,
                    SelectedCategoryIds = b.Categories.Select(c => c.Id).ToList(),
                    Authors = _authorService.GetAll().Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name }).ToList(),
                    Categories = _categoryService.GetAll().Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList()
                }).ToList();

            return View(books);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new BookViewModel
            {
                Authors = _authorService.GetAll().Where(a => !a.IsDeleted).Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name }).ToList(),
                Categories = _categoryService.GetAll().Where(c => !c.IsDeleted).Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Authors = _authorService.GetAll().Where(a => !a.IsDeleted).Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name }).ToList();
                model.Categories = _categoryService.GetAll().Where(c => !c.IsDeleted).Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
                return View(model);
            }

            string? imageUrl = null;
            if (model.ImageFile is { Length: > 0 })
            {
                var fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                var extension = Path.GetExtension(model.ImageFile.FileName);
                fileName = $"{fileName}_{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await model.ImageFile.CopyToAsync(stream);
                imageUrl = "/images/" + fileName;
            }

            var book = new Book
            {
                Title = model.Title,
                AuthorId = model.SelectedAuthorId,
                IsAvailableForRental = model.IsAvailableForRental,
                Hall = model.Hall,
                Publisher = model.Publisher,
                PublishingDate = model.PublishingDate,
                ImageUrl = imageUrl,
                Description = model.Description,
                CreatedOn = DateTime.Now,
                IsDeleted = false
            };

            if (model.SelectedCategoryIds is { Count: > 0 })
            {
                var selectedCategories = _categoryService.GetAll().Where(c => model.SelectedCategoryIds.Contains(c.Id)).ToList();
                book.Categories = selectedCategories;
            }

            _bookService.Add(book);
            _bookService.Complete();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var book = _bookService.GetByIdWithDetails(id.Value);
            if (book == null || book.IsDeleted) return NotFound();

            var model = new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                IsAvailableForRental = book.IsAvailableForRental,
                Hall = book.Hall,
                Publisher = book.Publisher,
                PublishingDate = book.PublishingDate,
                ImageUrl = book.ImageUrl,
                Description = book.Description,
                SelectedAuthorId = book.AuthorId,
                SelectedCategoryIds = book.Categories.Select(c => c.Id).ToList(),
                Authors = _authorService.GetAll().Where(a => !a.IsDeleted).Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name }).ToList(),
                Categories = _categoryService.GetAll().Where(c => !c.IsDeleted).Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Authors = _authorService.GetAll().Where(a => !a.IsDeleted).Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name }).ToList();
                model.Categories = _categoryService.GetAll().Where(c => !c.IsDeleted).Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
                return View(model);
            }

            var book = _bookService.GetByIdWithDetails(model.Id);
            if (book == null || book.IsDeleted) return NotFound();

            book.Title = model.Title;
            book.AuthorId = model.SelectedAuthorId;
            book.IsAvailableForRental = model.IsAvailableForRental;
            book.Hall = model.Hall;
            book.Publisher = model.Publisher;
            book.PublishingDate = model.PublishingDate;
            book.Description = model.Description;
            book.LastUpdatedOn = DateTime.Now;

            if (model.ImageFile is { Length: > 0 })
            {
                var fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                var extension = Path.GetExtension(model.ImageFile.FileName);
                fileName = $"{fileName}_{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await model.ImageFile.CopyToAsync(stream);
                book.ImageUrl = "/images/" + fileName;
            }

            book.Categories.Clear();
            if (model.SelectedCategoryIds is { Count: > 0 })
            {
                var selectedCategories = _categoryService.GetAll().Where(c => model.SelectedCategoryIds.Contains(c.Id)).ToList();
                book.Categories.AddRange(selectedCategories);
            }

            _bookService.Complete();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var book = _bookService.GetByIdWithDetails(id);
            if (book == null) return NotFound();

            var model = new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                IsAvailableForRental = book.IsAvailableForRental,
                Hall = book.Hall,
                Publisher = book.Publisher,
                PublishingDate = book.PublishingDate,
                ImageUrl = book.ImageUrl,
                Description = book.Description,
                SelectedAuthorId = book.AuthorId,
                SelectedCategoryIds = book.Categories.Select(c => c.Id).ToList(),
                BookCopies = book.BookCopies.Select(copy => new BookCopyViewModel
                {
                    Id = copy.Id,
                    BookTitle = book.Title,
                    IsAvailableForRental = copy.IsAvailableForRental,
                    EditionNumber = copy.EditionNumber,
                    SerialNumber = copy.SerialNumber,
                    IsDeleted = copy.IsDeleted
                }).ToList(),
                Authors = _authorService.GetAll().Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name,
                    Selected = a.Id == book.AuthorId
                }).ToList(),
                Categories = _categoryService.GetAll().Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = book.Categories.Any(bc => bc.Id == c.Id)
                }).ToList()
            };

            return View(model);
        }
    }
}
