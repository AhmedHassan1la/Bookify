namespace Bookify.Domain.Entities
{
    public class RentalBookCopy
    {
        public int RentalId { get; set; }
        public Rental? Rental { get; set; }
        public int BookCopyId { get; set; }
        public BookCopy? BookCopy { get; set; }
        public DateTime RentalDate { get; set; } = DateTime.Today;
        public DateTime RentalEnd { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime? ExtendedOn { get; set; }

        // خاصية لتحديد عدد الأيام
        public int RentalDurationInDays { get; set; } = 7; // القيمة الافتراضية 7 أيام

        // Constructor لتعيين RentalEnd بناءً على RentalDurationInDays
        public RentalBookCopy()
        {
            RentalEnd = RentalDate.AddDays(RentalDurationInDays);
        }
    }
}