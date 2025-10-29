using Bookify.Domain.Entities;

namespace Bookify.Application.Services.Categories
{
    public interface ICategoryService
    {
        IEnumerable<Domain.Entities.Category> GetAll();
        Domain.Entities.Category? GetById(int id);
        void Add(Domain.Entities.Category category);
        void Update(Domain.Entities.Category category);
        bool IsNameUnique(string name, int id = 0);
        void ToggleStatus(int id);
    }
}
