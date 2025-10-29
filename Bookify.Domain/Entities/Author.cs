using Bookify.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Domain.Entities
{

    [Index(nameof(Name), IsUnique = true)]
    public class Author : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string Des { get; set; } = null!;


    }
}
