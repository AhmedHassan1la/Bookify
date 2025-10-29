using Bookify.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Bookify.Domain.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Governorate : BaseEntity
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = null!;

        public ICollection<Area> Areas { get; set; } = new List<Area>();
    }
}