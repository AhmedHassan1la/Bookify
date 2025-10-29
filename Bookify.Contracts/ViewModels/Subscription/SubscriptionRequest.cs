namespace Bookify.Contracts.ViewModels.Subscription
{
    public class SubscriptionRequest
    {
        public int SubscriberId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
