using System.ComponentModel.DataAnnotations;

namespace Bookify.Web.Core.ViewModels.BookCopy
{
    public class BookCopyViewModel
    {
        public int Id { get; set; }
        public int BookId { get; set; }

        public string? BookTitle { get; set; }
        public bool IsAvailableForRental { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Edition Number must be greater than or equal to 1.")]

        public int EditionNumber { get; set; }  //رقم الطابعه
        public int SerialNumber { get; set; }   // ده هيابه unique  ولما هاجي اعمل search عن ال BookCopy هستخدم ال SerialNumber
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? LastUpdatedOn { get; set; }
    }
}
