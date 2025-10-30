using Bookify.Domain.Common;

namespace Bookify.Domain.Entities
{
    public class Rental : BaseEntity
    {
        public int Id { get; set; }

        // علاقة مع المشترك
        public int SubscriberId { get; set; }
        public int SubscriberId1 { get; set; }
        public Subscriber Subscriber { get; set; } = null!;



        // تاريخ بدء الاستئجار
        public DateTime StartDate { get; set; } = DateTime.Today;

        public bool PenaltyPaid { get; set; }
        public ICollection<RentalBookCopy> RentalBookCopies { get; set; } = new List<RentalBookCopy>();

    }
}
