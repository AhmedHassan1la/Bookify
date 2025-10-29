using Bookify.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Domain.Entities
{

    [Index(nameof(Name), IsUnique = true)]
    public class Category : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<Book> Books { get; set; } = new List<Book>();

    }
}
