using Bookify.Application.Common.Interfaces;
using Bookify.Application.Services.Subscribers;
using Bookify.Contracts.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bookify.Contracts.ViewModels.Subscriber;
using Bookify.Contracts.ViewModels.Subscription;
using Bookify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Bookify.Application.Services.Subscribers
{
    public class SubscriberService : ISubscriberService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubscriberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<SubscriberViewModel> SearchSubscribers(string? searchTerm)
        {
            var query = _unitOfWork.SubscribersQueryable;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(s =>
                    s.FirstName.Contains(searchTerm) ||
                    s.LastName.Contains(searchTerm) ||
                    s.MobileNumber.Contains(searchTerm) ||
                    s.NationalId.Contains(searchTerm));
            }

            return query.Select(s => new SubscriberViewModel
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                MobileNumber = s.MobileNumber
            }).ToList();
        }

        public SubscriberViewModel GetCreateViewModel()
        {
            var governorates = _unitOfWork.SubscribersQueryable
                .Select(s => s.Governorate)
                .Distinct()
                .ToList();

            var areas = _unitOfWork.SubscribersQueryable
                .Select(s => s.Area)
                .Distinct()
                .ToList();

            return new SubscriberViewModel
            {
                Governorates = new SelectList(governorates, "Id", "Name"),
                Areas = new SelectList(areas, "Id", "Name")
            };
        }

        public bool CreateSubscriber(SubscriberViewModel model)
        {
            var subscriber = new Subscriber
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                NationalId = model.NationalId,
                MobileNumber = model.MobileNumber,
                HasWhatsApp = model.HasWhatsApp,
                Email = model.Email,
                ImageUrl = model.ImageUrl,
                AreaId = model.AreaId,
                GovernorateId = model.GovernorateId,
                Address = model.Address,
                IsBlackListed = model.IsBlackListed,
                CreatedOn = DateTime.UtcNow
            };

            _unitOfWork.Subscribers.Add(subscriber);
            _unitOfWork.Complete();

            return true;
        }

        public SubscriberViewModel? GetSubscriberDetails(int id)
        {
            var subscriber = _unitOfWork.SubscribersQueryable
                .Include(s => s.Governorate)
                .Include(s => s.Area)
                .Include(s => s.Subscriptions)
                .FirstOrDefault(s => s.Id == id);

            if (subscriber == null) return null;

            return new SubscriberViewModel
            {
                Id = subscriber.Id,
                FirstName = subscriber.FirstName,
                LastName = subscriber.LastName,
                DateOfBirth = subscriber.DateOfBirth,
                NationalId = subscriber.NationalId,
                MobileNumber = subscriber.MobileNumber,
                HasWhatsApp = subscriber.HasWhatsApp,
                Email = subscriber.Email,
                ImageUrl = subscriber.ImageUrl,
                Address = subscriber.Address,
                GovernorateId = subscriber.GovernorateId,
                Governorates = new SelectList(new[] { subscriber.Governorate }, "Id", "Name"),
                AreaId = subscriber.AreaId,
                Areas = new SelectList(new[] { subscriber.Area }, "Id", "Name"),
                IsBlackListed = subscriber.IsBlackListed,
                Subscriptions = subscriber.Subscriptions!.Select(s => new SubscriptionViewModel
                {
                    Id = s.Id,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    Name = s.Name
                }).ToList()
            };
        }

        public (bool Success, string Message) AddSubscription(SubscriptionRequest request)
        {
            if (request.SubscriberId <= 0)
                return (false, "Invalid subscriber ID.");

            if (!DateTime.TryParseExact(request.StartDate, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime start) ||
                !DateTime.TryParseExact(request.EndDate, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime end))
            {
                return (false, "Invalid date format. Expected: yyyy-MM-ddTHH:mm:ss.fffZ");
            }

            if (end <= start)
                return (false, "End date must be later than start date.");

            var subscriber = _unitOfWork.SubscribersQueryable
                .Include(s => s.Subscriptions)
                .FirstOrDefault(s => s.Id == request.SubscriberId);

            if (subscriber == null)
                return (false, $"Subscriber with ID {request.SubscriberId} not found.");

            foreach (var subscription in subscriber.Subscriptions)
            {
                if (subscription.EndDate < DateTime.Today)
                    subscription.IsActive = false;
            }

            var newSubscription = new Subscription
            {
                Name = "New Subscription",
                StartDate = start,
                EndDate = end,
                IsActive = true
            };

            subscriber.Subscriptions.Add(newSubscription);
            _unitOfWork.Complete();

            return (true, "Subscription added successfully!");
        }
    }
}
