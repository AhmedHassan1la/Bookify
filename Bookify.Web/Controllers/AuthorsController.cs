using Bookify.Application.Services.Authors;
using Bookify.Domain.Entities;
using Bookify.Web.Core.ViewModels.Author;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Web.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        public IActionResult Index()
        {
            var authors = _authorService.GetAll();

            var viewModel = authors.Select(c => new AuthorViewModel
            {
                Id = c.Id,
                Name = c.Name,
                IsDeleted = c.IsDeleted,
                CreatedOn = c.CreatedOn,
                LastUpdatedOn = c.LastUpdatedOn
            }).ToList();

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateAuthorViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (!_authorService.IsNameUnique(model.Name))
            {
                ModelState.AddModelError("Name", "Author name already exists.");
                return View(model);
            }

            var author = new Author { Name = model.Name };
            _authorService.Add(author);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var author = _authorService.GetById(id);
            if (author == null) return NotFound();

            var viewModel = new EditViewModel
            {
                Id = author.Id,
                Name = author.Name
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (!_authorService.IsNameUnique(model.Name, model.Id))
            {
                ModelState.AddModelError("Name", "Author name already exists.");
                return View(model);
            }

            var author = _authorService.GetById(model.Id);
            if (author == null) return NotFound();

            author.Name = model.Name;
            author.LastUpdatedOn = DateTime.Now;
            _authorService.Update(author);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult ToggleStatus(int id)
        {
            var author = _authorService.GetById(id);
            if (author == null) return NotFound();

            _authorService.ToggleStatus(author);

            return Json(new { lastUpdatedOn = author.LastUpdatedOn.ToString(), isDeleted = author.IsDeleted });
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult IsAuthorNameUnique(string name, int id = 0)
        {
            return Json(_authorService.IsNameUnique(name, id));
        }
    }
}
