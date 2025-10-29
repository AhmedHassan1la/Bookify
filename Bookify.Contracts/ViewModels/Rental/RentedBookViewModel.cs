namespace Bookify.Contracts.ViewModels.Rental
{
    public class RentedBookViewModel
    {
        public int SerialNumber { get; set; }
        public int RentedBy { get; set; }
        public string? BookTitle { get; set; }
        public int EditionNumber { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime RentalEnd { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
