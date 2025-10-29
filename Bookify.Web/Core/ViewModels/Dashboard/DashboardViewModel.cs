namespace Bookify.Web.Core.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        // 📚 عدد الكتب في النظام
        public int TotalBooks { get; set; }

        // 👥 عدد المشتركين المسجلين
        public int TotalSubscribers { get; set; }
        public int TotalBookCopies { get; set; }

        // 🔄 عدد الاشتراكات النشطة
        public int ActiveSubscriptions { get; set; }

        // 📖 عدد النسخ المتاحة من الكتب (Book Copies)
        public int AvailableBookCopies { get; set; }

        // 🚫 عدد النسخ المحجوزة أو المستأجرة حاليًا
        public int RentedBookCopies { get; set; }

        // 🏷️ عدد عمليات التأجير الجارية حاليًا
        public int ActiveRentals { get; set; }

        // ❌ عدد المشتركين المحظورين (Blacklisted)
        public int BlacklistedSubscribers { get; set; }

        // 📅 عدد الاشتراكات التي انتهت صلاحيتها
        public int ExpiredSubscriptions { get; set; }

        // 🔝 أكثر 5 كتب تأجيرًا
        public List<TopBookViewModel> TopRentedBooks { get; set; } = new List<TopBookViewModel>();

        // 📆 عدد عمليات التأجير خلال آخر 30 يومًا
        public int RentalsLast30Days { get; set; }

        // 🔄 عدد الكتب المسترجعة خلال آخر 30 يومًا
        public int ReturnedBooksLast30Days { get; set; }

        // 📊 إحصائيات حسب الأشهر (عدد التأجيرات في كل شهر)
        public Dictionary<string, int> MonthlyRentals { get; set; } = new Dictionary<string, int>();

        // 📈 إحصائيات حسب الفئات الأكثر تأجيرًا
        public Dictionary<string, int> CategoryRentals { get; set; } = new Dictionary<string, int>();
    }

    // نموذج لأكثر الكتب تأجيرًا
    public class TopBookViewModel
    {
        public string Title { get; set; } = null!;
        public int RentalCount { get; set; }
    }
}
