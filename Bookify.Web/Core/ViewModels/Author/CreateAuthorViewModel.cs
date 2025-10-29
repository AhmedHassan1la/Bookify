using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Bookify.Web.Core.ViewModels.Author
{
    public class CreateAuthorViewModel
    {
        [Required(ErrorMessage = "Author name is required.")]
        [StringLength(100, ErrorMessage = "Author name must be less than 100 characters.")]
        [Remote(action: "IsAuthorNameUnique", controller: "Authors", ErrorMessage = "Author name already exists.")]
        public string Name { get; set; }
    }
}
