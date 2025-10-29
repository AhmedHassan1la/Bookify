using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Bookify.Web.Core.ViewModels.Author
{
    public class EditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Author name is required.")]
        [StringLength(100, ErrorMessage = "Author name must be less than 100 characters.")]
        [Remote(action: "IsAuthorNameUnique", controller: "Authors", AdditionalFields = "Id", ErrorMessage = "Author name already exists.")]
        public string Name { get; set; }
    }
}
