using Bookify.Application.Common.Interfaces;
using Bookify.Application.Services.Authors;
using Bookify.Domain.Entities;

namespace Bookify.Application.Services.Authors
{
    internal class AuthorService : IAuthorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Author> GetAll() => _unitOfWork.Authors.GetAll();

        public Author? GetById(int id) => _unitOfWork.Authors.GetById(id);

        public void Add(Author author)
        {
            _unitOfWork.Authors.Add(author);
            _unitOfWork.Complete();
        }

        public void Update(Author author)
        {
            _unitOfWork.Authors.Update(author);
            _unitOfWork.Complete();
        }

        public bool IsNameUnique(string name, int id = 0)
        {
            return !_unitOfWork.Authors.Any(c => c.Name == name && c.Id != id);
        }

        public void ToggleStatus(Author author)
        {
            author.IsDeleted = !author.IsDeleted;
            author.LastUpdatedOn = DateTime.Now;
            _unitOfWork.Complete();
        }
    }
}
