using Bookify.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Bookify.Domain.Entities
{
    public class Subscription : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Subscription Name")]
        public string Name { get; set; } = null!;

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [MaxLength(500)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        // Relation to Subscriber
        public int SubscriberId { get; set; }
        public Subscriber? Subscriber { get; set; }
    }
}
