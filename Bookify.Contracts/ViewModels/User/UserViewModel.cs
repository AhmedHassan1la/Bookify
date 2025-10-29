using Bookify.Domain.Consts;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Bookify.Contracts.ViewModels.User
{
    public class UserViewModel
    {





        public string Id { get; set; } = null!;
        [MaxLength(100, ErrorMessage = Errors.MaxLength), Display(Name = "Full Name"),
            RegularExpression(RegexPatterns.CharactersOnly_Eng, ErrorMessage = Errors.OnlyEnglishLetters)]
        public string FullName { get; set; } = null!;

        [MaxLength(20, ErrorMessage = Errors.MaxLength), Display(Name = "Username"),
 RegularExpression(RegexPatterns.Username, ErrorMessage = Errors.InvalidUsername)]
        public string UserName { get; set; } = null!;

        [MaxLength(200, ErrorMessage = Errors.MaxLength), EmailAddress]
        public string Email { get; set; } = null!;



        [DataType(DataType.Password),
                  StringLength(100, ErrorMessage = Errors.MaxMinLength, MinimumLength = 8),
                  RegularExpression(RegexPatterns.Password, ErrorMessage = Errors.WeakPassword)]
        // [RequiredIf("Id == null", ErrorMessage = Errors.RequiredField)]
        public string? Password { get; set; } = null!;

        [DataType(DataType.Password), Display(Name = "Confirm password"),
            Compare("Password", ErrorMessage = Errors.ConfirmPasswordNotMatch)]
        // [RequiredIf("Id == null", ErrorMessage = Errors.RequiredField)]
        public string? ConfirmPassword { get; set; } = null!;

        [Display(Name = "Roles")]
        public IList<string> SelectedRoles { get; set; } = new List<string>();

        public IEnumerable<SelectListItem>? Roles { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public bool IsLockedOut { get; set; }

    }
}
