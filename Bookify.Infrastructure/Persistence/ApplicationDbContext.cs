using Bookify.Application.Common.Interfaces;
using Bookify.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>,IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BookCopy> BookCopies { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<RentalBookCopy> RentalBookCopies { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.HasSequence<int>("SerialNumber")
        .StartsAt(100)
        .IncrementsBy(1);

            builder.Entity<BookCopy>(entity =>
            {
                entity.Property(b => b.SerialNumber)
                      .HasDefaultValueSql("NEXT VALUE FOR SerialNumber");

                entity.HasIndex(b => b.SerialNumber)
                      .IsUnique();  // Index لعدم التكرار
            });

            builder.Entity<Subscriber>()
       .HasOne(s => s.Area)
       .WithMany()
       .HasForeignKey(s => s.AreaId)
       .OnDelete(DeleteBehavior.Restrict);  // أو DeleteBehavior.NoAction

            builder.Entity<Subscriber>()
                .HasOne(s => s.Governorate)
                .WithMany()
                .HasForeignKey(s => s.GovernorateId)
                .OnDelete(DeleteBehavior.Restrict);  // أو DeleteBehavior.NoAction

            // فرض فريدة البريد الإلكتروني واسم المستخدم

            // تكوين العلاقة Many-to-Many بين Rental و BookCopy باستخدام RentalBookCopy كجدول وسيط
            builder.Entity<RentalBookCopy>()
                .HasKey(rb => new { rb.RentalId, rb.BookCopyId }); // تعيين مفتاح مركب

            // تكوين العلاقة بين Rental و RentalBookCopy
            builder.Entity<RentalBookCopy>()
                .HasOne(rb => rb.Rental)
                .WithMany(r => r.RentalBookCopies)
                .HasForeignKey(rb => rb.RentalId);

            // تكوين العلاقة بين BookCopy و RentalBookCopy
            builder.Entity<RentalBookCopy>()
                .HasOne(rb => rb.BookCopy)
                .WithMany(bc => bc.RentalBookCopies)
                .HasForeignKey(rb => rb.BookCopyId);

            base.OnModelCreating(builder);

        }

    }
}
