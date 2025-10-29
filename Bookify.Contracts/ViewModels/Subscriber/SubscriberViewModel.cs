using Bookify.Contracts.ViewModels.Subscription;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Bookify.Contracts.ViewModels.Subscriber
{
    public class SubscriberViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(100, ErrorMessage = "First name cannot exceed 100 characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(100, ErrorMessage = "Last name cannot exceed 100 characters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "National ID is required.")]
        [MaxLength(20, ErrorMessage = "National ID cannot exceed 20 characters.")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "National ID must be 14 digits.")]
        [Display(Name = "National ID")]
        public string NationalId { get; set; } = null!;

        [Required(ErrorMessage = "Mobile number is required.")]
        [MaxLength(15, ErrorMessage = "Mobile number cannot exceed 15 characters.")]
        [RegularExpression(@"^01[0-9]{9}$", ErrorMessage = "Mobile number must start with 01 and be 11 digits.")]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; } = null!;

        [Display(Name = "Has WhatsApp")]
        public bool HasWhatsApp { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [MaxLength(150, ErrorMessage = "Email cannot exceed 150 characters.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }

        [MaxLength(500), Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Area is required.")]
        [Display(Name = "Area")]
        public int AreaId { get; set; }
        public SelectList? Areas { get; set; }

        [Required(ErrorMessage = "Governorate is required.")]
        [Display(Name = "Governorate")]
        public int GovernorateId { get; set; }
        public SelectList? Governorates { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [MaxLength(500, ErrorMessage = "Address cannot exceed 500 characters.")]
        [Display(Name = "Address")]
        public string Address { get; set; } = null!;

        [Display(Name = "Is Blacklisted")]
        public bool IsBlackListed { get; set; }
        public bool IsDeleted { get; set; }

        // Subscription Information (List of subscriptions for this subscriber)
        [Display(Name = "Subscriptions")]
        public List<SubscriptionViewModel> Subscriptions { get; set; } = new List<SubscriptionViewModel>();
    }
}
