using Bookify.Application.Common.Interfaces;
using Bookify.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Application.Services.Categories
{
    internal class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Category> GetAll() => _unitOfWork.Categories.GetAll();

        public Category? GetById(int id) => _unitOfWork.Categories.GetById(id);

        public void Add(Category category)
        {
            _unitOfWork.Categories.Add(category);
            _unitOfWork.Complete();
        }

        public void Update(Category category)
        {
            _unitOfWork.Categories.Update(category);
            _unitOfWork.Complete();
        }

        public void ToggleStatus(int id)
        {
            var category = _unitOfWork.Categories.GetById(id);
            if (category is null) return;

            category.IsDeleted = !category.IsDeleted;
            category.LastUpdatedOn = DateTime.Now;
            _unitOfWork.Complete();
        }

        public bool IsNameUnique(string name, int id = 0)
        {
            return !_unitOfWork.Categories.Any(c => c.Name == name && c.Id != id);
        }
    }
}
