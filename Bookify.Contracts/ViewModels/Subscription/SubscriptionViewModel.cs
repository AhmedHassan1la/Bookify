using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Bookify.Contracts.ViewModels.Subscription
{
    public class SubscriptionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Subscription name is required.")]
        [MaxLength(100, ErrorMessage = "Subscription name cannot exceed 100 characters.")]
        [Display(Name = "Subscription Name")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Price is required.")]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Subscriber is required.")]
        [Display(Name = "Subscriber")]
        public int SubscriberId { get; set; }
        public SelectList? Subscribers { get; set; } // For dropdown list of subscribers
    }
}
