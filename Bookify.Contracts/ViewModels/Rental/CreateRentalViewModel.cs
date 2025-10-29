namespace Bookify.Contracts.ViewModels.Rental
{
    public class CreateRentalViewModel
    {
        public int SubscriberId { get; set; }
        public List<int> SerialNumbers { get; set; } = new List<int> { 0, 0, 0 }; // تهيئة القائمة بـ 3 قيم
    }

}
