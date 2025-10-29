using Bookify.Domain.Entities;

namespace Bookify.Domain.Common
{
    public class BaseEntity
    {
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? LastUpdatedOn { get; set; }

        public string? CreatedById { get; set; } // ده row مين أنشأ الـ
        public ApplicationUser? CreatedBy { get; set; }

        public string? LastUpdatedById { get; set; } // ده row مين حدث الـ
        public ApplicationUser? LastUpdatedBy { get; set; }
    }
}
