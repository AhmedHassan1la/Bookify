using Bookify.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Bookify.Domain.Entities
{
    [Index(nameof(Name), nameof(GovernorateId), IsUnique = true)]
    public class Area : BaseEntity
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = null!;
        public string Name1 { get; set; } = null!;

        public int GovernorateId { get; set; }

        public Governorate? Governorate { get; set; }
    }
}
