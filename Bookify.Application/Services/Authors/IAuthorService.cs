using Bookify.Domain.Entities;
using System.Collections.Generic;

namespace Bookify.Application.Services.Authors
{
    public interface IAuthorService
    {
        IEnumerable<Author> GetAll();
        Author? GetById(int id);
        void Add(Author author);
        void Update(Author author);
        bool IsNameUnique(string name, int id = 0);
        void ToggleStatus(Author author);
    }
}
