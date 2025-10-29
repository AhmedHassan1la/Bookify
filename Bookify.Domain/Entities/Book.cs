using Bookify.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Domain.Entities
{
    [Index(nameof(Title), nameof(AuthorId), IsUnique = true)]
    public class Book : BaseEntity
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public Author? Author { get; set; }

        public string Title { get; set; } = null!;
        public bool IsAvailableForRental { get; set; }
        public int Hall { get; set; }
        public string Publisher { get; set; } = null!;
        public DateTime PublishingDate { get; set; }
        public string? ImageUrl { get; set; }
        public string Description { get; set; } = null!;

        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<BookCopy> BookCopies { get; set; } = new List<BookCopy>();

        // علاقة مع Rental
        public ICollection<Rental>? Rentals { get; set; } = new List<Rental>();  // أضفنا علاقة مع Rental
    }
}
