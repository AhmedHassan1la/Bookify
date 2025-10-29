using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Bookify.Web.Core.ViewModels.Category
{
    public class EditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, ErrorMessage = "Category name must be less than 100 characters.")]
        [Remote(action: "IsCategoryNameUnique", controller: "Categories", AdditionalFields = "Id", ErrorMessage = "Category name already exists.")]
        public string Name { get; set; }
    }
}
