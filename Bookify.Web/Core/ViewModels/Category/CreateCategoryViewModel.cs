using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Bookify.Web.Core.ViewModels.Category
{
    public class CreateCategoryViewModel
    {
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, ErrorMessage = "Category name must be less than 100 characters.")]
        [Remote(action: "IsCategoryNameUnique", controller: "Categories", ErrorMessage = "Category name already exists.")]
        public string Name { get; set; }
    }
}
