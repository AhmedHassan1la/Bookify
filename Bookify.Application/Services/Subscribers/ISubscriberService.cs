using Bookify.Contracts.ViewModels;
using Bookify.Domain.Entities;
using Bookify.Contracts.ViewModels.Subscriber;
using Bookify.Contracts.ViewModels.Subscription;

namespace Bookify.Application.Services.Subscribers
{
    public interface ISubscriberService
    {
        List<SubscriberViewModel> SearchSubscribers(string? searchTerm);
        SubscriberViewModel GetCreateViewModel();
        bool CreateSubscriber(SubscriberViewModel model);
        SubscriberViewModel? GetSubscriberDetails(int id);
        (bool Success, string Message) AddSubscription(SubscriptionRequest request);
    }
}
