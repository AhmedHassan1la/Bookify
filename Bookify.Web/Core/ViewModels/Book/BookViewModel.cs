using Bookify.Web.Core.ViewModels.BookCopy;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Bookify.Web.Core.ViewModels.Book
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public bool IsAvailableForRental { get; set; }
        public int Hall { get; set; }
        public string Publisher { get; set; } = null!;
        public DateTime PublishingDate { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; } // ملف الصورة الذي يتم تحميله

        public string Description { get; set; } = null!;

        // المؤلف المحدد (يتم تعبئته من القائمة المنسدلة)
        [Display(Name = "SelectedAuthor")]
        public int SelectedAuthorId { get; set; }

        // قائمة المؤلفين للاختيار منها
        public List<SelectListItem> Authors { get; set; } = new List<SelectListItem>();

        // التصنيفات المحددة (يتم تعبئتها من القائمة المنسدلة)
        [Display(Name = "SelectedCategories")]

        public List<int> SelectedCategoryIds { get; set; } = new List<int>();

        // قائمة التصنيفات للاختيار منها
        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        public List<BookCopyViewModel> BookCopies { get; set; } = new List<BookCopyViewModel>();  // إضافة جميع النسخ هنا


    }
}
