using Bookify.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Domain.Entities
{
    [Index(nameof(EditionNumber), IsUnique = true)]
    public class BookCopy : BaseEntity
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public bool IsAvailableForRental { get; set; }
        public int EditionNumber { get; set; }  //رقم الطابعه
        public int SerialNumber { get; private set; }  // Readonly في الكود
                                                       // ده هيابه unique  ولما هاجي اعمل search عن ال BookCopy هستخدم ال SerialNumber
        public ICollection<RentalBookCopy> RentalBookCopies { get; set; } = new List<RentalBookCopy>();



    }
}
