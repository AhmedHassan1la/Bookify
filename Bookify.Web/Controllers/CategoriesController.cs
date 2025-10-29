using Bookify.Application.Services.Categories;
using Bookify.Domain.Entities;
using Bookify.Web.Core.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            var categories = _categoryService.GetAll();

            var viewModel = categories.Select(c => new CategoryViewModel
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
        public IActionResult Create(CreateCategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (!_categoryService.IsNameUnique(model.Name))
            {
                ModelState.AddModelError("Name", "Category name already exists.");
                return View(model);
            }

            var category = new Category { Name = model.Name };
            _categoryService.Add(category);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _categoryService.GetById(id);
            if (category is null)
                return NotFound();

            var viewModel = new EditViewModel
            {
                Id = category.Id,
                Name = category.Name
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (!_categoryService.IsNameUnique(model.Name, model.Id))
            {
                ModelState.AddModelError("Name", "Category name already exists.");
                return View(model);
            }

            var category = _categoryService.GetById(model.Id);
            if (category is null)
                return NotFound();

            category.Name = model.Name;
            category.LastUpdatedOn = DateTime.Now;

            _categoryService.Update(category);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult ToggleStatus(int id)
        {
            _categoryService.ToggleStatus(id);
            var category = _categoryService.GetById(id);

            return Json(new
            {
                lastUpdatedOn = category?.LastUpdatedOn?.ToString(),
                isDeleted = category?.IsDeleted
            });
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult IsCategoryNameUnique(string name, int id = 0)
        {
            var isUnique = _categoryService.IsNameUnique(name, id);
            return Json(isUnique);
        }
    }
}
